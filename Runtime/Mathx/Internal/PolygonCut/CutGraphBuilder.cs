using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SensenToolkit.Internal
{
    public static class CutGraphBuilder
    {
        public static CutGraph Build(Polygon2D polygon, SimpleSegment2D cutSegment)
        {
            Assertx.IsTrue(cutSegment.IsInfiniteBothSides, "Cut segment must be infinite on both sides");

            Vector2 origin = cutSegment.Position;
            Vector2 direction = cutSegment.Direction;
            Vector2[] vertices = polygon.Vertices;
            SimpleSegment2D[] segments = polygon.Segments;
            List<CutGraphNode> cutNodesList = new();
            List<CutGraphNode> allNodesList = new();
            CutGraphNode lastNode = null;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertex = vertices[i];
                Vector2 nextVertex = vertices[(i + 1) % vertices.Length];
                SimpleSegment2D segment = segments[i];
                Vector2 cutPerp = new(-direction.y, direction.x);
                float cutPerpProjection = Vector2.Dot(cutPerp, vertex - origin);

                bool isCuttingVertex = cutSegment.Contains(vertex);
                Segment2DIntersectionResult intersectResult = cutSegment.Intersect(segment);

                if (!isCuttingVertex)
                {
                    var vertexNode = new CutGraphNode
                    {
                        Position = vertex,
                        IsCutIntersection = false,
                        PreviousNode = lastNode,
                        NextNode = null,
                        IsSideA = cutPerpProjection <= 0 || Mathf.Approximately(cutPerpProjection, 0),
                        IsSideB = cutPerpProjection >= 0 || Mathf.Approximately(cutPerpProjection, 0)
                    };
                    allNodesList.Add(vertexNode);
                    if (lastNode != null) lastNode.NextNode = vertexNode;
                    lastNode = vertexNode;
                }

                if (isCuttingVertex || (intersectResult.IsIntersecting && !cutSegment.Contains(nextVertex)))
                {
                    var cutNode = new CutGraphNode
                    {
                        Position = isCuttingVertex ? vertex : intersectResult.ProjectionPoint,
                        IsCutIntersection = true,
                        IsAVertexThatWasCut = isCuttingVertex,
                        PreviousNode = lastNode,
                        NextNode = null,
                        IsSideA = true,
                        IsSideB = true
                    };
                    allNodesList.Add(cutNode);
                    cutNodesList.Add(cutNode);
                    if (lastNode != null) lastNode.NextNode = cutNode;
                    lastNode = cutNode;
                }
            }
            lastNode.NextNode = allNodesList[0];
            allNodesList[0].PreviousNode = lastNode;

            CutGraphNode[] allNodes = allNodesList.ToArray();
            allNodesList = null;

            CutGraphNode[] cutNodes = cutNodesList
                .OrderBy(cp => Vector2.Dot(cp.Position - origin, direction))
                .ToArray();

            cutNodesList.Clear();
            for (int i = 0; i < cutNodes.Length; i++)
            {
                CutGraphNode cutNode = cutNodes[i];
                CutGraphNode previousCutNode = i == 0 ? null : cutNodes[i - 1];
                CutGraphNode nextCutNode = i == cutNodes.Length - 1 ? null : cutNodes[i + 1];
                bool hasSegmentToNextCutNode = cutNode.NextNode == nextCutNode
                    || cutNode.PreviousNode == nextCutNode;
                bool hasSegmentToPreviousCutNode = cutNode.NextNode == previousCutNode
                    || cutNode.PreviousNode == previousCutNode;
                if (hasSegmentToNextCutNode && hasSegmentToPreviousCutNode)
                {
                    previousCutNode.NextNode = nextCutNode;
                    nextCutNode.PreviousNode = previousCutNode;
                }
                else
                {
                    cutNodesList.Add(cutNode);
                }
            }

            cutNodes = cutNodesList.ToArray();
            cutNodesList = null;

            bool onSideAPolygon = false;
            bool onSideBPolygon = false;
            for (int i = 0; i < cutNodes.Length; i++)
            {
                CutGraphNode cutNode = cutNodes[i];
                if (!cutNode.NextNode.IsAVertexThatWasCut)
                {
                    if (cutNode.NextNode.IsSideA) cutNode.SideABranchNodes.Add(cutNode.NextNode);
                    if (cutNode.NextNode.IsSideB) cutNode.SideBBranchNodes.Add(cutNode.NextNode);
                }
                if (!cutNode.PreviousNode.IsAVertexThatWasCut)
                {
                    if (cutNode.PreviousNode.IsSideA) cutNode.SideABranchNodes.Add(cutNode.PreviousNode);
                    if (cutNode.PreviousNode.IsSideB) cutNode.SideBBranchNodes.Add(cutNode.PreviousNode);
                }

                if (cutNode.IsAVertexThatWasCut)
                {
                    cutNode.IsSideA = onSideAPolygon || cutNode.HasBranchToSideA;
                    cutNode.IsSideB = onSideBPolygon || cutNode.HasBranchToSideB;
                }

                if (i >= 1 && (onSideAPolygon || onSideBPolygon))
                {
                    CutGraphNode previousCutNode = cutNodes[i - 1];
                    previousCutNode.NextCrossingCutNode = cutNode;
                    cutNode.PreviousCrossingCutNode = previousCutNode;
                }

                if (cutNode.HasOneBranchToSideA) onSideAPolygon = !onSideAPolygon;
                if (cutNode.HasOneBranchToSideB) onSideBPolygon = !onSideBPolygon;
            }

            return new CutGraph(allNodes.ToArray(), cutNodes);
        }
    }
}
