using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class TributaryCalculator : ITributaryCalculator
{
    private const double ParallelDotProductThreshold = 0.95;
    private const double MinOverlapInches = 12.0;
    private const double MinNeighborDistanceInches = 1.0;
    private const double InchesPerFoot = 12.0;
    private const int FloorSamplePoints = 5;

    public IReadOnlyList<LinearTributary> CalculateForBeam(LoadableBeam beam, StructuralModel model)
    {
        var sourceBeam = model.Elements.Beams.FirstOrDefault(b => b.Id == beam.MemberId);
        if (sourceBeam?.StartPoint is null || sourceBeam.EndPoint is null)
            return [];

        var beamStart = new Vec2(sourceBeam.StartPoint.X, sourceBeam.StartPoint.Y);
        var beamEnd = new Vec2(sourceBeam.EndPoint.X, sourceBeam.EndPoint.Y);
        var beamVec = beamEnd - beamStart;
        if (beamVec.IsZero)
            return [];

        var beamDir = beamVec.Normalized();

        var candidates = CollectParallelOverlappingNeighbors(sourceBeam, beamStart, beamEnd, beamDir, model);

        var results = new List<LinearTributary>();
        results.AddRange(BuildSide(sourceBeam, beamStart, beamEnd, beamDir, candidates, sideA: true, model));
        results.AddRange(BuildSide(sourceBeam, beamStart, beamEnd, beamDir, candidates, sideA: false, model));
        return results;
    }

    private static List<Segment> CollectParallelOverlappingNeighbors(
        Beam sourceBeam, Vec2 beamStart, Vec2 beamEnd, Vec2 beamDir, StructuralModel model)
    {
        var raw = new List<Segment>();

        foreach (var other in model.Elements.Beams)
        {
            if (ReferenceEquals(other, sourceBeam) || other.Id == sourceBeam.Id) continue;
            if (other.LevelId != sourceBeam.LevelId) continue;
            if (other.StartPoint is null || other.EndPoint is null) continue;
            raw.Add(new Segment(
                new Vec2(other.StartPoint.X, other.StartPoint.Y),
                new Vec2(other.EndPoint.X, other.EndPoint.Y)));
        }

        // Walls "on the same level" as a beam are walls whose top is at that level
        // (i.e. the beam sits on top of the wall on this floor plan).
        foreach (var wall in model.Elements.Walls)
        {
            if (wall.TopLevelId != sourceBeam.LevelId) continue;
            if (wall.StartPoint is null || wall.EndPoint is null) continue;
            raw.Add(new Segment(
                new Vec2(wall.StartPoint.X, wall.StartPoint.Y),
                new Vec2(wall.EndPoint.X, wall.EndPoint.Y)));
        }

        var filtered = new List<Segment>(raw.Count);
        foreach (var seg in raw)
        {
            var segVec = seg.B - seg.A;
            if (segVec.IsZero) continue;
            if (!AreParallel(beamDir, segVec.Normalized())) continue;
            if (OverlapAlong(beamStart, beamEnd, seg.A, seg.B) < MinOverlapInches) continue;
            filtered.Add(seg);
        }
        return filtered;
    }

    private static IEnumerable<LinearTributary> BuildSide(
        Beam sourceBeam, Vec2 beamStart, Vec2 beamEnd, Vec2 beamDir,
        List<Segment> parallelNeighbors, bool sideA, StructuralModel model)
    {
        var perp = sideA
            ? new Vec2(-beamDir.Y, beamDir.X)
            : new Vec2(beamDir.Y, -beamDir.X);

        var beamMid = (beamStart + beamEnd) * 0.5;

        double nearest = double.MaxValue;
        foreach (var seg in parallelNeighbors)
        {
            var candMid = (seg.A + seg.B) * 0.5;
            if (Vec2.Dot(candMid - beamMid, perp) <= 0) continue;
            double d = PerpendicularDistance(beamStart, seg.A, seg.B);
            if (d < MinNeighborDistanceInches) continue;
            if (d < nearest) nearest = d;
        }

        double tribWidthInches = nearest < double.MaxValue ? nearest / 2.0 : 0.0;
        double tribWidthFt = tribWidthInches / InchesPerFoot;

        var surfaceLoads = SampleSurfaceLoads(sourceBeam.LevelId, beamStart, beamEnd, perp, tribWidthInches, model);

        if (surfaceLoads.Count == 0)
        {
            yield return new LinearTributary
            {
                Width = tribWidthFt,
                SurfaceLoadId = string.Empty,
                DeadLoadPsf = 0,
                LiveLoadPsf = 0
            };
            yield break;
        }

        foreach (var sl in surfaceLoads)
        {
            yield return new LinearTributary
            {
                Width = tribWidthFt,
                SurfaceLoadId = sl.Id,
                DeadLoadPsf = sl.DeadLoadValue ?? 0,
                LiveLoadPsf = sl.LiveLoadValue ?? 0
            };
        }
    }

    private static List<SurfaceLoad> SampleSurfaceLoads(
        string levelId, Vec2 beamStart, Vec2 beamEnd, Vec2 perp,
        double tribWidthInches, StructuralModel model)
    {
        var result = new List<SurfaceLoad>();
        if (tribWidthInches <= 0 || string.IsNullOrEmpty(levelId)) return result;

        var floors = new List<Floor>();
        foreach (var f in model.Elements.Floors)
            if (f.LevelId == levelId) floors.Add(f);
        if (floors.Count == 0) return result;

        var halfStripOffset = perp * (tribWidthInches * 0.5);
        var seen = new HashSet<string>();

        for (int i = 0; i < FloorSamplePoints; i++)
        {
            double t = (i + 0.5) / FloorSamplePoints;
            var alongBeam = beamStart * (1 - t) + beamEnd * t;
            var sample = alongBeam + halfStripOffset;

            foreach (var floor in floors)
            {
                if (floor.Points is null || floor.Points.Count < 3) continue;
                if (!PointInPolygon(sample, floor.Points)) continue;
                if (string.IsNullOrEmpty(floor.SurfaceLoadId)) break;
                if (!seen.Add(floor.SurfaceLoadId)) break;
                var sl = model.Loads.SurfaceLoads.FirstOrDefault(s => s.Id == floor.SurfaceLoadId);
                if (sl is not null) result.Add(sl);
                break;
            }
        }
        return result;
    }

    private static bool AreParallel(Vec2 a, Vec2 b)
    {
        if (a.IsZero || b.IsZero) return false;
        return Math.Abs(Vec2.Dot(a, b)) >= ParallelDotProductThreshold;
    }

    private static double PerpendicularDistance(Vec2 fromPoint, Vec2 toA, Vec2 toB)
    {
        var d = toB - toA;
        double l2 = d.X * d.X + d.Y * d.Y;
        if (l2 < 1e-12) return (fromPoint - toA).Length;
        double t = ((fromPoint.X - toA.X) * d.X + (fromPoint.Y - toA.Y) * d.Y) / l2;
        var proj = new Vec2(toA.X + t * d.X, toA.Y + t * d.Y);
        return (fromPoint - proj).Length;
    }

    private static double OverlapAlong(Vec2 beamStart, Vec2 beamEnd, Vec2 candA, Vec2 candB)
    {
        var d = beamEnd - beamStart;
        double l2 = d.X * d.X + d.Y * d.Y;
        if (l2 < 1e-12) return 0;
        double beamLen = Math.Sqrt(l2);
        double t1 = ((candA.X - beamStart.X) * d.X + (candA.Y - beamStart.Y) * d.Y) / l2;
        double t2 = ((candB.X - beamStart.X) * d.X + (candB.Y - beamStart.Y) * d.Y) / l2;
        if (t1 > t2) (t1, t2) = (t2, t1);
        double overlapStart = Math.Max(0, t1);
        double overlapEnd = Math.Min(1, t2);
        if (overlapEnd <= overlapStart) return 0;
        return (overlapEnd - overlapStart) * beamLen;
    }

    private static bool PointInPolygon(Vec2 p, List<Point> poly)
    {
        bool inside = false;
        int n = poly.Count;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            var pi = poly[i];
            var pj = poly[j];
            double dy = pj.Y - pi.Y;
            if (Math.Abs(dy) < 1e-18) continue;
            if ((pi.Y > p.Y) != (pj.Y > p.Y) &&
                p.X < (pj.X - pi.X) * (p.Y - pi.Y) / dy + pi.X)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    private readonly record struct Segment(Vec2 A, Vec2 B);

    private readonly record struct Vec2(double X, double Y)
    {
        public double Length => Math.Sqrt(X * X + Y * Y);
        public bool IsZero => Length < 1e-9;
        public Vec2 Normalized() { var l = Length; return l < 1e-9 ? new Vec2(0, 0) : new Vec2(X / l, Y / l); }
        public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator *(Vec2 a, double s) => new(a.X * s, a.Y * s);
        public static double Dot(Vec2 a, Vec2 b) => a.X * b.X + a.Y * b.Y;
    }
}
