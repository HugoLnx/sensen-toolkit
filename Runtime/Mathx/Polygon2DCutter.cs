using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SensenToolkit.Internal;

namespace SensenToolkit
{
    public class Polygon2DCutter
    {
        private SimpleSegment2D _cutSegment;
        private Polygon2D _polygon;
        private CutGraph _graph;
        private List<CutPolygonBuilder> _allPolygons = new();
        private HashSet<(CutGraphNode, bool)> _usedCutNodes = new();

        public Polygon2DCutter(Polygon2D polygon, Vector2 origin, Vector2 direction)
        {
            _polygon = polygon;
            _cutSegment = new SimpleSegment2D(
                position: origin,
                direction: direction,
                lengthForward: Mathf.Infinity,
                lengthBackward: Mathf.Infinity
            );
            _graph = CutGraphBuilder.Build(polygon, _cutSegment);
        }

        public (List<Polygon2D>, List<Polygon2D>) Execute()
        {
            if (_graph.AllNodes.Length <= 2) throw new InvalidOperationException("Polygon must have at least 3 vertices");
            if (_graph.CutNodes.Length == 0)
            {
                List<Polygon2D> sideA = new();
                List<Polygon2D> sideB = new();
                (_graph.AllNodes[0].IsSideA ? sideA : sideB).Add(_polygon);
                return (sideA, sideB);
            }

            bool anyNodeIsInSideA = false;
            bool anyNodeIsInSideB = false;
            foreach (CutGraphNode node in _graph.CutNodes)
            {
                anyNodeIsInSideA = anyNodeIsInSideA || node.IsSideA;
                anyNodeIsInSideB = anyNodeIsInSideB || node.IsSideB;
                if (anyNodeIsInSideA && anyNodeIsInSideB) break;
            }
            if (!anyNodeIsInSideB) return (new List<Polygon2D> { _polygon }, new());
            if (!anyNodeIsInSideA) return (new(), new List<Polygon2D> { _polygon });

            foreach (CutGraphNode cutNode in _graph.CutNodes)
            {
                MountPolygon(
                    firstCutNode: cutNode,
                    isSideA: true
                );

                MountPolygon(
                    firstCutNode: cutNode,
                    isSideA: false
                );
            }


            List<Polygon2D> sideAPolygons = new();
            List<Polygon2D> sideBPolygons = new();
            foreach (CutPolygonBuilder mounting in _allPolygons)
            {
                Polygon2D polygon = mounting.BuildPolygon();
                List<Polygon2D> polygonsList = mounting.IsSideA
                    ? sideAPolygons
                    : sideBPolygons;
                polygonsList.Add(polygon);
            }

            return (sideAPolygons, sideBPolygons);
        }

        private void MountPolygon(CutGraphNode firstCutNode, bool isSideA)
        {
            if (_usedCutNodes.Contains((firstCutNode, isSideA))) return;

            bool isSideB = !isSideA;
            if (isSideA && !firstCutNode.IsSideA) return;
            if (isSideB && !firstCutNode.IsSideB) return;

            CutPolygonBuilder polygon = new();
            polygon.IsSideA = isSideA;
            _allPolygons.Add(polygon);

            CutGraphNode lastNode = null;
            CutGraphNode node = firstCutNode;
            do
            {
                polygon.AddAtEnd(node);
                if (node.IsCutIntersection) _usedCutNodes.Add((node, isSideA));

                CutGraphNode nodeBkp = node;
                node = GetNextPolygonNode(node, lastNode, isSideA);
                lastNode = nodeBkp;
            } while (node != firstCutNode);
        }

        private CutGraphNode GetNextPolygonNode(CutGraphNode node, CutGraphNode lastNode, bool isSideA)
        {
            CutGraphNode nextNode = node.NextNode;
            CutGraphNode prevNode = node.PreviousNode;
            CutGraphNode crossCutPrevNode = node.PreviousCrossingCutNode;
            CutGraphNode crossCutNextNode = node.NextCrossingCutNode;
            if (nextNode != lastNode && !IsCutSegment(node, nextNode) && CheckSide(nextNode, isSideA)) return nextNode;
            if (prevNode != lastNode && !IsCutSegment(node, prevNode) && CheckSide(prevNode, isSideA)) return prevNode;
            if (crossCutPrevNode != null && crossCutPrevNode != lastNode && CheckSide(crossCutPrevNode, isSideA)) return crossCutPrevNode;
            if (crossCutNextNode != null && crossCutNextNode != lastNode && CheckSide(crossCutNextNode, isSideA)) return crossCutNextNode;
            throw new InvalidOperationException($"No next polygon node found for {node.Position} in side {(isSideA ? "A" : "B")}");
        }

        private bool IsCutSegment(CutGraphNode a, CutGraphNode b)
        {
            return (a.NextNode == b || a.PreviousNode == b) && (a.IsCutIntersection && b.IsCutIntersection);
        }

        private bool CheckSide(CutGraphNode node, bool isSideA)
        {
            if (isSideA && node.IsSideA) return true;
            if (!isSideA && node.IsSideB) return true;
            return false;
        }
    }
}
