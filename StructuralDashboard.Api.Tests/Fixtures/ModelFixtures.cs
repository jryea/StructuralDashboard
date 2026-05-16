using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Services;
using StructuralDashboard.Shared.Contracts;
using StructuralDashboard.Shared.Shared;

namespace StructuralDashboard.Api.Tests.Fixtures;

/// <summary>
/// Hand-crafted small structural models for behavioral testing of the loading pipeline.
/// Every dimension is in inches; surface loads are psf.
/// </summary>
public static class ModelFixtures
{
    public const string LevelL1 = "L1";
    public const string LevelL2 = "L2";
    public const string SurfaceLoad50 = "SL-50";
    public const string SurfaceLoad100 = "SL-100";

    /// <summary>
    /// One-level model with two walls supporting a single beam, covered by one floor.
    ///
    /// Plan (top view, looking down at L2):
    ///   Wall W1 runs along y=0,   x=0..240   (TopLevelId = L2)
    ///   Wall W2 runs along y=120, x=0..240   (TopLevelId = L2)
    ///   Beam B1 runs along y=60,  x=0..240   (LevelId = L2)
    ///   Floor F1 covers the full plan rectangle 0..240 × 0..120 at L2.
    /// Both walls are parallel to the beam; perpendicular distance = 60" → tributary 30" → 2.5 ft each side.
    /// SurfaceLoad 50 psf DL + 50 psf LL → 100 psf total → 100 * 2.5 * 2 sides = 500 plf.
    /// </summary>
    public static StructuralModel WallSupportedBeam()
    {
        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W1", x1: 0, y1: 0, x2: 240, y2: 0, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Walls.Add(NewWall("W2", x1: 0, y1: 120, x2: 240, y2: 120, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { 0.0, 240, 240, 0 }, ys: new[] { 0.0, 0, 120, 120 }));
        return model;
    }

    /// <summary>
    /// A beam with NO parallel neighbors on either side and no surrounding floor.
    /// Tributary width must be 0 on both sides.
    /// </summary>
    public static StructuralModel IsolatedBeam()
    {
        var model = NewBaseModel();
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        return model;
    }

    /// <summary>
    /// Beam with a parallel neighbor on ONE side only.
    /// W2 is on the +Y side at distance 120"; nothing on the -Y side.
    /// Floor F1 covers the entire plan (so both sides "have" a floor).
    /// </summary>
    public static StructuralModel BeamWithOneSidedNeighbor()
    {
        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W2", x1: 0, y1: 180, x2: 240, y2: 180, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { 0.0, 240, 240, 0 }, ys: new[] { -120.0, -120, 240, 240 }));
        return model;
    }

    /// <summary>
    /// Beam with multiple parallel candidates on the +Y side.
    /// W-near at y=120 (distance 60), W-far at y=240 (distance 180).
    /// Tributary on +Y side should choose the nearest (W-near), width = 30" = 2.5 ft.
    /// </summary>
    public static StructuralModel BeamWithMultipleCandidatesOnOneSide()
    {
        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W-near", x1: 0, y1: 120, x2: 240, y2: 120, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Walls.Add(NewWall("W-far", x1: 0, y1: 240, x2: 240, y2: 240, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Walls.Add(NewWall("W-back", x1: 0, y1: -120, x2: 240, y2: -120, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { 0.0, 240, 240, 0 }, ys: new[] { -240.0, -240, 360, 360 }));
        return model;
    }

    /// <summary>
    /// Beam with a beam on one side and a wall on the other.
    /// B-other at y=120 (distance 60, perpendicular) and W1 at y=0 (distance 60).
    /// Both sides get tributary 30" = 2.5 ft.
    /// </summary>
    public static StructuralModel BeamWithMixedNeighbors()
    {
        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W1", x1: 0, y1: 0, x2: 240, y2: 0, topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B-other", x1: 0, y1: 120, x2: 240, y2: 120, z: 120, levelId: LevelL2));
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { 0.0, 240, 240, 0 }, ys: new[] { 0.0, 0, 120, 120 }));
        return model;
    }

    /// <summary>
    /// Beam at a 30° angle to the X-axis. Two walls parallel to it, 60" perpendicular on either side.
    /// Tests that perpendicular distance is correctly handled for non-axis-aligned framing.
    /// </summary>
    public static StructuralModel AngledBeamWithParallelWalls()
    {
        // Direction of beam: cos(30°) ≈ 0.866, sin(30°) ≈ 0.5
        // Perpendicular: (-sin, cos) = (-0.5, 0.866). Half-width 60" offset gives:
        //   side+ : start (0+(-30), 0+51.96), end (240*0.866 + (-30), 240*0.5 + 51.96)
        //   side- : start (0+30,    0-51.96), end (240*0.866 + 30,    240*0.5 - 51.96)
        const double c = 0.866025403784, s = 0.5;
        const double half = 60.0;
        var px = -s * half;
        var py = c * half;

        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W-plus",
            x1: px, y1: py, x2: 240 * c + px, y2: 240 * s + py,
            topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Walls.Add(NewWall("W-minus",
            x1: -px, y1: -py, x2: 240 * c - px, y2: 240 * s - py,
            topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B1",
            x1: 0, y1: 0, x2: 240 * c, y2: 240 * s, z: 120, levelId: LevelL2));
        // Floor large enough to contain everything
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { -200.0, 400, 400, -200 }, ys: new[] { -200.0, -200, 400, 400 }));
        return model;
    }

    /// <summary>
    /// Beam with a parallel neighbor that does NOT overlap in projection along the beam's axis.
    /// W-disjoint runs at y=120 but only from x=300..540 (beam is x=0..240) — no overlap.
    /// The disjoint neighbor must be skipped → +Y side tributary = 0.
    /// </summary>
    public static StructuralModel BeamWithNonOverlappingNeighbor()
    {
        var model = NewBaseModel();
        model.Elements.Walls.Add(NewWall("W-disjoint", x1: 300, y1: 120, x2: 540, y2: 120,
            topLevelId: LevelL2, baseLevelId: LevelL1));
        model.Elements.Beams.Add(NewBeam("B1", x1: 0, y1: 60, x2: 240, y2: 60, z: 120, levelId: LevelL2));
        model.Elements.Floors.Add(NewFloor("F1", levelId: LevelL2, surfaceLoadId: SurfaceLoad50,
            xs: new[] { -120.0, 600, 600, -120 }, ys: new[] { 0.0, 0, 240, 240 }));
        return model;
    }

    /// <summary>
    /// Two-level model for accumulator testing.
    /// L1 (z=0) and L2 (z=120) and L3 (z=240).
    /// Joist J at L3 lands on Girder G mid-span; G is supported by columns at its ends.
    /// Used to verify upstream→downstream processing order.
    /// </summary>
    public static StructuralModel TwoLevelJoistOnGirder()
    {
        var model = NewBaseModel();
        model.ModelLayout.Levels.Add(new Level { Id = "L3", Name = "Level 3", Elevation = 240 });

        // L3 framing
        // Girder G at L3, x=0..240, y=60 (top)
        model.Elements.Beams.Add(NewBeam("G", x1: 0, y1: 60, x2: 240, y2: 60, z: 240, levelId: "L3"));
        // Joist J at L3, x=120, y=0..120 → its endpoints land on Girder's mid-span (120, 60)
        // and on a wall at y=0.
        model.Elements.Beams.Add(NewBeam("J", x1: 120, y1: 0, x2: 120, y2: 120, z: 240, levelId: "L3"));

        // Walls support the joist's far end and the girder at its endpoints
        model.Elements.Walls.Add(NewWall("W-joist-end", x1: 0, y1: 0, x2: 240, y2: 0,
            topLevelId: "L3", baseLevelId: "L1"));

        // Columns at girder ends
        model.Elements.Columns.Add(NewColumn("C-G-start", x: 0, y: 60, zBottom: 0, zTop: 240));
        model.Elements.Columns.Add(NewColumn("C-G-end", x: 240, y: 60, zBottom: 0, zTop: 240));

        // Floor at L3 (above the framing). For tributary, beams at L3 with LevelId="L3".
        model.Elements.Floors.Add(NewFloor("F-L3", levelId: "L3", surfaceLoadId: SurfaceLoad50,
            xs: new[] { 0.0, 240, 240, 0 }, ys: new[] { 0.0, 0, 120, 120 }));

        return model;
    }

    public static StructuralGraph BuildGraph(StructuralModel model)
    {
        var stub = new InMemoryModelService(model);
        var service = new StructuralGraphService(stub);
        return service.GetOrBuildGraphAsync(model.Id).GetAwaiter().GetResult();
    }

    // ------------------------------------------------------------
    // Builders
    // ------------------------------------------------------------

    private static StructuralModel NewBaseModel()
    {
        var model = new StructuralModel
        {
            Id = "MDL-FIXTURE-001"
        };
        model.ModelLayout.Levels = new List<Level>
        {
            new() { Id = LevelL1, Name = "Level 1", Elevation = 0 },
            new() { Id = LevelL2, Name = "Level 2", Elevation = 120 }
        };
        model.Loads.SurfaceLoads.Add(new SurfaceLoad
        {
            Id = SurfaceLoad50,
            Name = "50/50",
            DeadLoadValue = 50,
            LiveLoadValue = 50
        });
        model.Loads.SurfaceLoads.Add(new SurfaceLoad
        {
            Id = SurfaceLoad100,
            Name = "100/100",
            DeadLoadValue = 100,
            LiveLoadValue = 100
        });
        return model;
    }

    private static Beam NewBeam(string id, double x1, double y1, double x2, double y2, double z, string levelId) =>
        new()
        {
            Id = id,
            StartPoint = new Point { X = x1, Y = y1, Z = z },
            EndPoint = new Point { X = x2, Y = y2, Z = z },
            LevelId = levelId
        };

    private static Wall NewWall(string id, double x1, double y1, double x2, double y2, string topLevelId, string baseLevelId) =>
        new()
        {
            Id = id,
            StartPoint = new Point { X = x1, Y = y1, Z = 0 },
            EndPoint = new Point { X = x2, Y = y2, Z = 0 },
            BaseLevelId = baseLevelId,
            TopLevelId = topLevelId
        };

    private static Column NewColumn(string id, double x, double y, double zBottom, double zTop) =>
        new()
        {
            Id = id,
            StartPoint = new Point { X = x, Y = y, Z = zBottom },
            EndPoint = new Point { X = x, Y = y, Z = zTop }
        };

    private static Floor NewFloor(string id, string levelId, string surfaceLoadId, double[] xs, double[] ys)
    {
        var floor = new Floor
        {
            Id = id,
            LevelId = levelId,
            SurfaceLoadId = surfaceLoadId,
            Points = new List<Point>()
        };
        for (int i = 0; i < xs.Length; i++)
            floor.Points.Add(new Point { X = xs[i], Y = ys[i], Z = 0 });
        return floor;
    }

    private sealed class InMemoryModelService : IStructuralModelService
    {
        private readonly StructuralModel _model;
        public InMemoryModelService(StructuralModel model) => _model = model;
        public Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber) => Task.FromResult(new List<StructuralModel> { _model });
        public Task<StructuralModel?> GetModelAsync(string modelId) => Task.FromResult<StructuralModel?>(modelId == _model.Id ? _model : null);
        public Task CreateModelAsync(StructuralModel model, string projectNumber) => Task.CompletedTask;
        public Task UpdateModelAsync(StructuralModel model) => Task.CompletedTask;
        public Task DeleteModelAsync(string modelId) => Task.CompletedTask;
    }
}
