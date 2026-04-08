namespace StructuralDashboard.Web.Data;

public static class DummyPlanData
{
    public static PlanViewModel GetModel()
    {
        return new PlanViewModel
        {
            Members = new List<PlanMember>
            {
                new PlanMember() {Id = "b-001", Type ="beam", X1 = 0.0, Y1 = 0.0, X2 = 20.0, Y2 = 0.0},
                new PlanMember() {Id = "b-002", Type ="beam", X1 = 20.0, Y1 = 0.0, X2 = 20.0, Y2 = 20.0},
                new PlanMember() {Id = "b-003", Type ="beam", X1 = 20.0, Y1 = 20.0, X2 = 0.0, Y2 = 20.0},
                new PlanMember() {Id = "b-004", Type ="beam", X1 = 0.0, Y1 = 20.0, X2 = 0.0, Y2 = 0.0},
            }
        };
    }
}
