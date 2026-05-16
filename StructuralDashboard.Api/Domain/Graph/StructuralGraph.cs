namespace StructuralDashboard.Api.Domain.Graph;

public class StructuralGraph
{
    public string ModelId { get; set; }

    // All derived nodes keyed by node ID for O(1) lookup
    // example: N-001": {Id: "N-001", ....}
    public Dictionary<string, GraphNode> Nodes { get; set; } = new();

    // Member ID → its start and end node IDs
    // Lets us find a member's nodes without scanning the whole graph
    //"BM-girder-001": {
    //  "id": "BM-girder-001",
    //  "nodeIds": ["N-005", "N-012", "N-018", "N-006"]
    public Dictionary<string, GraphMember> Members { get; set; } = new();

    // Quick set lookup for support detection during traversal
    // Redundant with Nodes but avoids iterating all nodes to find supports
    public HashSet<string> SupportNodeIds { get; set; } = new();
}