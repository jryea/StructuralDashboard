namespace StructuralDashboard.Api.Domain;

public class GraphNode
{
    public string Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public NodeType NodeType { get; set; }

    // All member IDs connected to this node
    public List<string> ConnectedMemberIds { get; set; } = new();

    // Semantic level reference — nullable because nodes between levels
    // (mid-column splice points etc) may not map cleanly to a level
    public string? LevelId { get; set; }

    // Z rounded to a tolerance bucket for elevation comparison
    // Derived during graph construction, not stored in source data
    public double NominalElevation { get; set; }

}

public enum NodeType
{
    Joint,          // standard member-to-member connection
    Support,        // foundation connection — load terminates here
    FreeEnd,        // single member connects — cantilever tip etc
}
