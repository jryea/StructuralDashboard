using StructuralDashboard.Api.Domain.Graph;

namespace StructuralDashboard.Api.Services;

public class StructuralGraphService : IStructuralGraphService
{
    private readonly IStructuralModelService _modelService;

    // In-memory cache for now — key is modelId
    // TODO: Replace with Redis when caching layer is added
    private readonly Dictionary<string, StructuralGraph> _cache = new();

    // Two points are the same node if they're within this distance
    private const double TOLERANCE = 0.1;

    // Maximum perpendicular distance from a node to a member's line for the node
    // to be considered "on" the member. Same tolerance as node coincidence.
    private const double ON_LINE_TOLERANCE = 0.1;

    public StructuralGraphService(IStructuralModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<StructuralGraph> GetOrBuildGraphAsync(string modelId)
    {
        if (_cache.TryGetValue(modelId, out var cached))
            return cached;

        var model = await _modelService.GetModelAsync(modelId);

        if (model is null)
            throw new KeyNotFoundException($"Model {modelId} not found");

        var graph = BuildGraph(model);
        _cache[modelId] = graph;

        return graph;
    }

    public async Task<LoadPath> GetLoadPathAsync(string modelId, string memberId)
    {
        var graph = await GetOrBuildGraphAsync(modelId);

        if (!graph.Members.TryGetValue(memberId, out var member))
            throw new KeyNotFoundException($"Member {memberId} not found in graph");

        var upstream = new List<string>();
        var downstream = new List<string>();

        // Seed traversal from every node along the selected member
        // (start, end, and any intersection points along its length)
        foreach (var seedNodeId in member.NodeIds)
        {
            TraverseDirection(graph, seedNodeId, memberId, isDownstream: true, downstream);
            TraverseDirection(graph, seedNodeId, memberId, isDownstream: false, upstream);
        }

        return new LoadPath
        {
            SelectedMemberId = memberId,
            Upstream = upstream.Distinct().ToList(),
            Downstream = downstream.Distinct().ToList()
        };
    }

    private StructuralGraph BuildGraph(StructuralModel model)
    {
        var graph = new StructuralGraph { ModelId = model.Id };

        var allMemberGeometry = GetAllMembers(model);

        // Step 1 — derive nodes from coincident endpoints
        var pointToNodeId = new Dictionary<string, string>();
        var nodeCounter = 0;

        foreach (var (memberId, startPoint, endPoint) in allMemberGeometry)
        {
            var startKey = PointKey(startPoint.X, startPoint.Y, startPoint.Z);
            var endKey = PointKey(endPoint.X, endPoint.Y, endPoint.Z);

            if (!pointToNodeId.ContainsKey(startKey))
            {
                var nodeId = $"N-{nodeCounter++:D3}";
                pointToNodeId[startKey] = nodeId;
                graph.Nodes[nodeId] = CreateNode(nodeId, startPoint.X, startPoint.Y, startPoint.Z);
            }

            if (!pointToNodeId.ContainsKey(endKey))
            {
                var nodeId = $"N-{nodeCounter++:D3}";
                pointToNodeId[endKey] = nodeId;
                graph.Nodes[nodeId] = CreateNode(nodeId, endPoint.X, endPoint.Y, endPoint.Z);
            }

            var startNodeId = pointToNodeId[startKey];
            var endNodeId = pointToNodeId[endKey];

            // Step 2 — create the member with its initial two-node list
            graph.Members[memberId] = new GraphMember
            {
                Id = memberId,
                NodeIds = new List<string> { startNodeId, endNodeId }
            };

            graph.Nodes[startNodeId].ConnectedMemberIds.Add(memberId);
            graph.Nodes[endNodeId].ConnectedMemberIds.Add(memberId);
        }

        // Step 3 — find existing nodes that lie along OTHER members' lines
        // (joist-end-node falls on a girder's mid-span — case 1 from the design)
        InjectIntermediateNodes(graph, allMemberGeometry);

        // Step 4 — classify nodes and populate support set
        foreach (var node in graph.Nodes.Values)
        {
            node.NodeType = ClassifyNode(node);

            if (node.NodeType == NodeType.Support)
                graph.SupportNodeIds.Add(node.Id);
        }

        return graph;
    }

    // For each member, find any existing graph nodes (other than its own endpoints)
    // that lie on its line segment. Insert them into the member's node list in
    // geometric order, and register the member in each such node's connection list.
    private void InjectIntermediateNodes(
        StructuralGraph graph,
        List<(string MemberId, Point StartPoint, Point EndPoint)> allMemberGeometry)
    {
        var memberGeomLookup = allMemberGeometry.ToDictionary(m => m.MemberId);

        foreach (var (memberId, startPoint, endPoint) in allMemberGeometry)
        {
            var member = graph.Members[memberId];
            var startNodeId = member.NodeIds[0];
            var endNodeId = member.NodeIds[1];

            // For each candidate node, check if it falls on this member's line
            // (skipping the member's own two endpoint nodes)
            var hitsOnThisMember = new List<(string NodeId, double T)>();

            foreach (var (nodeId, node) in graph.Nodes)
            {
                if (nodeId == startNodeId || nodeId == endNodeId)
                    continue;

                if (TryGetParameterIfOnSegment(
                        startPoint, endPoint,
                        node.X, node.Y, node.Z,
                        out var t))
                {
                    hitsOnThisMember.Add((nodeId, t));
                }
            }

            if (hitsOnThisMember.Count == 0) continue;

            // Sort by t (0 = start, 1 = end) so the inserted nodes appear in
            // geometric order along the member's line
            hitsOnThisMember.Sort((a, b) => a.T.CompareTo(b.T));

            // Insert between start and end
            // Final order: [start, hit_0, hit_1, ..., hit_n, end]
            var newNodeIds = new List<string> { startNodeId };
            foreach (var (nodeId, _) in hitsOnThisMember)
            {
                newNodeIds.Add(nodeId);
                // Register the member in the intermediate node's connection list
                if (!graph.Nodes[nodeId].ConnectedMemberIds.Contains(memberId))
                    graph.Nodes[nodeId].ConnectedMemberIds.Add(memberId);
            }
            newNodeIds.Add(endNodeId);

            member.NodeIds = newNodeIds;
        }
    }

    // Tests whether a point lies on the segment from start to end.
    // If yes, returns the parameter t in [0, 1] along the segment.
    // Excludes the endpoints themselves (t very close to 0 or 1).
    private bool TryGetParameterIfOnSegment(
        Point start, Point end,
        double px, double py, double pz,
        out double t)
    {
        t = 0;

        // Vector from start to end
        var dx = end.X - start.X;
        var dy = end.Y - start.Y;
        var dz = end.Z - start.Z;

        // Vector from start to point
        var ex = px - start.X;
        var ey = py - start.Y;
        var ez = pz - start.Z;

        var segLengthSq = dx * dx + dy * dy + dz * dz;
        if (segLengthSq < 1e-12) return false; // degenerate segment

        // Parametric projection of P onto the line through start-end
        t = (ex * dx + ey * dy + ez * dz) / segLengthSq;

        // Exclude endpoints (within a small bracket — these are already nodes on this member)
        const double endpointBuffer = 0.01;
        if (t < endpointBuffer || t > 1.0 - endpointBuffer) return false;

        // Closest point on the line to P
        var closestX = start.X + t * dx;
        var closestY = start.Y + t * dy;
        var closestZ = start.Z + t * dz;

        // Perpendicular distance from P to the line
        var perpX = px - closestX;
        var perpY = py - closestY;
        var perpZ = pz - closestZ;
        var perpDistSq = perpX * perpX + perpY * perpY + perpZ * perpZ;

        return perpDistSq < ON_LINE_TOLERANCE * ON_LINE_TOLERANCE;
    }

    private List<(string MemberId, Point StartPoint, Point EndPoint)> GetAllMembers(StructuralModel model)
    {
        var members = new List<(string, Point, Point)>();

        foreach (var beam in model.Elements.Beams)
            members.Add((beam.Id, beam.StartPoint, beam.EndPoint));

        foreach (var column in model.Elements.Columns)
            members.Add((column.Id, column.StartPoint, column.EndPoint));

        foreach (var brace in model.Elements.Braces)
            members.Add((brace.Id, brace.StartPoint, brace.EndPoint));

        return members;
    }

    private GraphNode CreateNode(string id, double x, double y, double z)
    {
        return new GraphNode
        {
            Id = id,
            X = x,
            Y = y,
            Z = z,
            NominalElevation = Math.Round(z / TOLERANCE) * TOLERANCE
        };
    }

    private NodeType ClassifyNode(GraphNode node)
    {
        // Support — at or near grade
        if (Math.Abs(node.Z) < TOLERANCE)
            return NodeType.Support;

        // FreeEnd — only one member connects
        if (node.ConnectedMemberIds.Count == 1)
            return NodeType.FreeEnd;

        return NodeType.Joint;
    }

    private string PointKey(double x, double y, double z)
    {
        var snap = (double v) => Math.Round(v / TOLERANCE) * TOLERANCE;
        return $"{snap(x)},{snap(y)},{snap(z)}";
    }

    private void TraverseDirection(
        StructuralGraph graph,
        string seedNodeId,
        string selectedMemberId,
        bool isDownstream,
        List<string> result)
    {
        var visited = new HashSet<string> { selectedMemberId };

        // Tracks which (nodeId, memberId) pairs we've already processed
        // Prevents walking back-and-forth along the same member
        var processedAtNode = new HashSet<(string, string)>();

        var queue = new Queue<(string NodeId, string MemberId)>();

        SeedFromNode(graph, seedNodeId, selectedMemberId, isDownstream, visited, queue, result);

        while (queue.Count > 0)
        {
            var (nodeId, memberId) = queue.Dequeue();

            if (!processedAtNode.Add((nodeId, memberId))) continue;

            // Downstream stops at supports
            if (isDownstream && graph.SupportNodeIds.Contains(nodeId))
                continue;

            // 1. Branch out to other members at this node
            SeedFromNode(graph, nodeId, memberId, isDownstream, visited, queue, result);

            // 2. Walk along the arrived-on member to its other nodes
            // (essential for joist→girder where we land at an interior point)
            if (!graph.Members.TryGetValue(memberId, out var arrivedMember)) continue;
            foreach (var nextNodeOnMember in arrivedMember.GetAdjacentNodeIds(nodeId))
            {
                queue.Enqueue((nextNodeOnMember, memberId));
            }
        }
    }

    // At nodeId, having arrived via currentMemberId, decide which connecting members
    // to enqueue for this direction.
    private void SeedFromNode(
        StructuralGraph graph,
        string nodeId,
        string currentMemberId,
        bool isDownstream,
        HashSet<string> visited,
        Queue<(string, string)> queue,
        List<string> result)
    {
        if (!graph.Nodes.TryGetValue(nodeId, out var node)) return;

        var hasStrictDownAtNode = HasMemberInDirection(graph, node, isDownstream: true);
        var hasStrictUpAtNode = HasMemberInDirection(graph, node, isDownstream: false);

        // Are we currently on a member that passes THROUGH this node (interior)?
        var currentMemberIsThrough = false;
        if (graph.Members.TryGetValue(currentMemberId, out var currentMember))
        {
            var idx = currentMember.NodeIds.IndexOf(nodeId);
            currentMemberIsThrough = idx > 0 && idx < currentMember.NodeIds.Count - 1;
        }

        foreach (var connectedMemberId in node.ConnectedMemberIds)
        {
            if (connectedMemberId == currentMemberId) continue;
            if (visited.Contains(connectedMemberId)) continue;

            if (!graph.Members.TryGetValue(connectedMemberId, out var member)) continue;

            // Endpoint-vs-interior relationship at THIS node for the candidate member
            var idx = member.NodeIds.IndexOf(nodeId);
            if (idx < 0) continue;
            var candidateIsEndpointHere = idx == 0 || idx == member.NodeIds.Count - 1;

            // Classify direction of this candidate member relative to our traversal direction
            var direction = ClassifyDirection(
                graph, node, member,
                currentMemberIsThrough: currentMemberIsThrough,
                candidateIsEndpointHere: candidateIsEndpointHere,
                hasStrictDownAtNode: hasStrictDownAtNode,
                hasStrictUpAtNode: hasStrictUpAtNode);

            if (direction == null) continue;

            // Only follow members that match the traversal direction we're computing
            var matches = (isDownstream && direction == Direction.Downstream) ||
                          (!isDownstream && direction == Direction.Upstream);

            if (!matches) continue;

            // Enqueue from each adjacent node along the candidate member
            visited.Add(connectedMemberId);
            result.Add(connectedMemberId);

            foreach (var adjacentNodeId in member.GetAdjacentNodeIds(nodeId))
            {
                queue.Enqueue((adjacentNodeId, connectedMemberId));
            }
        }
    }

    private enum Direction { Upstream, Downstream }

    // Decide whether the candidate member flows downstream or upstream from this node
    // relative to the current traversal context.
    private Direction? ClassifyDirection(
        StructuralGraph graph,
        GraphNode node,
        GraphMember candidate,
        bool currentMemberIsThrough,
        bool candidateIsEndpointHere,
        bool hasStrictDownAtNode,
        bool hasStrictUpAtNode)
    {
        // Find the adjacent node on the candidate to determine elevation direction
        var adjacent = candidate.GetAdjacentNodeIds(node.Id).FirstOrDefault();
        if (adjacent == null) return null;
        if (!graph.Nodes.TryGetValue(adjacent, out var adjacentNode)) return null;

        var goesDown = adjacentNode.NominalElevation < node.NominalElevation - TOLERANCE;
        var goesUp = adjacentNode.NominalElevation > node.NominalElevation + TOLERANCE;
        var goesLateral = !goesDown && !goesUp;

        // Strict-direction members always win — if a column drops down from here, that's
        // the downstream. If a column rises, that's the upstream.
        if (goesDown) return Direction.Downstream;
        if (goesUp) return Direction.Upstream;

        // Same elevation — use endpoint-vs-interior to decide
        if (goesLateral)
        {
            // Case A: I'm passing through this node, candidate has its endpoint here
            // Candidate delivers to me — it's upstream
            if (currentMemberIsThrough && candidateIsEndpointHere)
                return Direction.Upstream;

            // Case B: My endpoint is here, candidate passes through
            // I deliver to candidate — it's downstream
            if (!currentMemberIsThrough && !candidateIsEndpointHere)
                return Direction.Downstream;

            // Case C: Both have endpoints here OR both pass through
            // Ambiguous — fall back to "do we have a strict-direction member at this node"
            // If yes, lateral members at this node are siblings, not load path. Drop them.
            // If no, treat as lateral propagation (could go either way; default to current direction).
            if (hasStrictDownAtNode || hasStrictUpAtNode)
                return null; // siblings — load delivered elsewhere

            // No strict directional member — keep walking laterally in the current direction
            // (this preserves the original "walk laterally until you find a down-path" behavior)
            return null;  // safer: don't classify ambiguous laterals at all
        }

        return null;
    }

    //private void SeedQueue(
    //    StructuralGraph graph,
    //    string nodeId,
    //    bool isDownstream,
    //    HashSet<string> visited,
    //    Queue<(string, string)> queue)
    //{
    //    if (!graph.Nodes.TryGetValue(nodeId, out var node))
    //        return;

    //    // Does this node have any member going strictly in the traversal direction?
    //    // If yes, the load is "delivered" here — only follow strict-direction members.
    //    var hasStrictlyDirectionalMember = HasMemberInDirection(graph, node, isDownstream);

    //    foreach (var connectedMemberId in node.ConnectedMemberIds)
    //    {
    //        if (visited.Contains(connectedMemberId))
    //            continue;

    //        // Find the adjacent node along this member from the current node
    //        if (!graph.Members.TryGetValue(connectedMemberId, out var member))
    //            continue;

    //        foreach (var adjacentNodeId in member.GetAdjacentNodeIds(nodeId))
    //        {
    //            if (!graph.Nodes.TryGetValue(adjacentNodeId, out var adjacentNode))
    //                continue;

    //            var goesStrictlyDownstream = adjacentNode.NominalElevation < node.NominalElevation;
    //            var goesStrictlyUpstream = adjacentNode.NominalElevation > node.NominalElevation;
    //            var goesLateral = Math.Abs(adjacentNode.NominalElevation - node.NominalElevation) < TOLERANCE;

    //            if (isDownstream)
    //            {
    //                if (hasStrictlyDirectionalMember)
    //                {
    //                    if (!goesStrictlyDownstream) continue;
    //                }
    //                else
    //                {
    //                    if (!goesStrictlyDownstream && !goesLateral) continue;
    //                }
    //            }
    //            else
    //            {
    //                if (hasStrictlyDirectionalMember)
    //                {
    //                    if (!goesStrictlyUpstream) continue;
    //                }
    //                else
    //                {
    //                    if (!goesStrictlyUpstream && !goesLateral) continue;
    //                }
    //            }

    //            if (!visited.Add(connectedMemberId)) continue; // already added by another adjacent check

    //            queue.Enqueue((adjacentNodeId, connectedMemberId));
    //            break; // one enqueue per member is enough; the rest will be reached via traversal
    //        }
    //    }
    //}

    private bool HasMemberInDirection(StructuralGraph graph, GraphNode node, bool isDownstream)
    {
        foreach (var memberId in node.ConnectedMemberIds)
        {
            if (!graph.Members.TryGetValue(memberId, out var member))
                continue;

            foreach (var adjacentNodeId in member.GetAdjacentNodeIds(node.Id))
            {
                if (!graph.Nodes.TryGetValue(adjacentNodeId, out var adjacentNode))
                    continue;

                if (isDownstream && adjacentNode.NominalElevation < node.NominalElevation)
                    return true;
                if (!isDownstream && adjacentNode.NominalElevation > node.NominalElevation)
                    return true;
            }
        }
        return false;
    }
}