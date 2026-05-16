using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;

namespace StructuralDashboard.Api.Tests.Services.Loading;

[TestFixture]
public class TributaryCalculatorTests
{
    private static LoadableBeam LoadableFor(string memberId) => new()
    {
        MemberId = memberId,
        StartNodeId = "n-ignored-start",
        EndNodeId = "n-ignored-end",
        Span = 20.0
    };

    [Test]
    public void HappyPath_BeamBetweenTwoParallelWalls_ProducesSymmetricTributary()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        Assert.That(tribs, Has.Count.EqualTo(2));
        // Both sides should have 2.5 ft width (60" gap / 2 = 30" = 2.5 ft)
        foreach (var t in tribs)
        {
            Assert.That(t.Width, Is.EqualTo(2.5).Within(1e-6));
            Assert.That(t.SurfaceLoadId, Is.EqualTo(ModelFixtures.SurfaceLoad50));
            Assert.That(t.DeadLoadPsf, Is.EqualTo(50));
            Assert.That(t.LiveLoadPsf, Is.EqualTo(50));
        }
        // 100 psf * 2.5 ft * 2 sides = 500 plf
        double totalPlf = tribs.Sum(t => t.ResultingLinePlf);
        Assert.That(totalPlf, Is.EqualTo(500).Within(1e-6));
    }

    [Test]
    public void OneSidedNeighbor_ZeroTributaryOnEmptySide()
    {
        var model = ModelFixtures.BeamWithOneSidedNeighbor();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // Exactly one side should have non-zero width
        Assert.That(tribs.Count(t => t.Width > 0), Is.EqualTo(1));
        Assert.That(tribs.Count(t => t.Width == 0), Is.EqualTo(1));

        var loaded = tribs.Single(t => t.Width > 0);
        // W2 sits at y=180, beam at y=60 → distance 120" → tributary 60" = 5 ft
        Assert.That(loaded.Width, Is.EqualTo(5.0).Within(1e-6));
    }

    [Test]
    public void NoNeighborsOnEitherSide_ReturnsBothSidesZero()
    {
        var model = ModelFixtures.IsolatedBeam();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // Two zero-width tributaries (one per side) — the calling service uses these
        // to know that both sides exist but have no neighbor.
        Assert.That(tribs, Has.Count.EqualTo(2));
        Assert.That(tribs.All(t => t.Width == 0), Is.True);
        Assert.That(tribs.Sum(t => t.ResultingLinePlf), Is.EqualTo(0));
    }

    [Test]
    public void MultipleCandidatesOnOneSide_PicksTheNearest()
    {
        var model = ModelFixtures.BeamWithMultipleCandidatesOnOneSide();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // Beam at y=60. Candidates on +Y: W-near at 120 (dist 60), W-far at 240 (dist 180).
        // Nearest wins: tributary = 60/2 = 30" = 2.5 ft.
        // -Y side: W-back at -120, distance 180 → tributary 7.5 ft.
        Assert.That(tribs.Any(t => Math.Abs(t.Width - 2.5) < 1e-6), Is.True,
            "Expected a 2.5 ft tributary from the nearest +Y neighbor (W-near)");
        Assert.That(tribs.Any(t => Math.Abs(t.Width - 7.5) < 1e-6), Is.True,
            "Expected a 7.5 ft tributary from the -Y neighbor (W-back)");
    }

    [Test]
    public void MixedNeighbors_BeamOnOneSideWallOnOther_BothContribute()
    {
        var model = ModelFixtures.BeamWithMixedNeighbors();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // W1 at y=0, B-other at y=120 — beam at y=60. Both 60" away.
        // Both sides → 30" = 2.5 ft tributary.
        Assert.That(tribs, Has.Count.EqualTo(2));
        Assert.That(tribs.All(t => Math.Abs(t.Width - 2.5) < 1e-6), Is.True);
    }

    [Test]
    public void BeamWithNoFloorAbove_EmitsZeroPlfTributary()
    {
        // Wall-supported beam (parallel neighbors found) but no floor on the level → no surface load.
        var model = ModelFixtures.WallSupportedBeam();
        model.Elements.Floors.Clear();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // Widths are present (neighbors exist) but no surface load → DL/LL = 0, plf = 0.
        Assert.That(tribs, Is.Not.Empty);
        Assert.That(tribs.All(t => t.Width > 0), Is.True);
        Assert.That(tribs.All(t => t.DeadLoadPsf == 0 && t.LiveLoadPsf == 0), Is.True);
        Assert.That(tribs.Sum(t => t.ResultingLinePlf), Is.EqualTo(0));
    }

    [Test]
    public void NonAxisAlignedBeam_PerpendicularDistanceIsCorrect()
    {
        var model = ModelFixtures.AngledBeamWithParallelWalls();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // Two walls offset 60" perpendicular → tributary 30" each side = 2.5 ft.
        Assert.That(tribs, Has.Count.EqualTo(2));
        foreach (var t in tribs)
            Assert.That(t.Width, Is.EqualTo(2.5).Within(1e-3));
    }

    [Test]
    public void NonOverlappingParallelNeighbor_IsSkipped()
    {
        var model = ModelFixtures.BeamWithNonOverlappingNeighbor();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("B1"), model);

        // W-disjoint doesn't overlap the beam's projection → skipped. Both sides should be zero.
        Assert.That(tribs.All(t => t.Width == 0), Is.True);
    }

    [Test]
    public void UnknownMemberId_ReturnsEmptyList()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var sut = new TributaryCalculator();

        var tribs = sut.CalculateForBeam(LoadableFor("does-not-exist"), model);
        Assert.That(tribs, Is.Empty);
    }
}
