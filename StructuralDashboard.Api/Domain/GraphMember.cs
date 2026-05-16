namespace StructuralDashboard.Api.Domain;

public class GraphMember
{
    public string Id { get; set; } = string.Empty;

    // Ordered list of node IDs along this member, from start to end.
    // For a simple member: [startNodeId, endNodeId]
    // For a girder with joists framing in: [startNodeId, joist1Node, joist2Node, endNodeId]
    // Order is geometrically meaningful — adjacent entries are adjacent along the member's line
    public List<string> NodeIds { get; set; } = new();

    // Returns the node(s) adjacent to the given node along this member
    // - If the given node is at an endpoint, returns one adjacent node (the next inward)
    // - If the given node is interior, returns both adjacent nodes (one in each direction)
    public IEnumerable<string> GetAdjacentNodeIds(string nodeId)
    {
        var index = NodeIds.IndexOf(nodeId);
        if (index < 0) yield break;

        if (index > 0) yield return NodeIds[index - 1];
        if (index < NodeIds.Count - 1) yield return NodeIds[index + 1];
    }
}