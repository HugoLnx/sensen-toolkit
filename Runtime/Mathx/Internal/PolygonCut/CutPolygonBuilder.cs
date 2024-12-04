using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SensenToolkit.Internal
{
    public class CutPolygonBuilder
    {
        public HashSet<CutGraphNode> Nodes { get; } = new();
        public List<Vector2> Vertices { get; } = new();
        public CutGraphNode StartCutNode { get; private set; }
        public bool IsSideA
        {
            get => _isSideA;
            set
            {
                _isSideA = value;
                _isSideB = !value;
            }
        }
        public bool IsSideB
        {
            get => _isSideB;
            set
            {
                _isSideA = !value;
                _isSideB = value;
            }
        }

        private bool _isSideA;
        private bool _isSideB;

        public bool AddAtEnd(CutGraphNode node)
        {
            if (Nodes.Contains(node)) return false;
            Nodes.Add(node);
            Vertices.Add(node.Position);
            if (Nodes.Count == 1) StartCutNode = node;
            return true;
        }

        public bool AddAtStart(CutGraphNode node)
        {
            if (Nodes.Contains(node)) return false;
            Nodes.Add(node);
            Vertices.Insert(0, node.Position);
            StartCutNode = node;
            return true;
        }

        public Polygon2D BuildPolygon()
        {
            return new Polygon2D(Vertices.ToArray());
        }
    }
}
