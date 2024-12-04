using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Internal
{
    public class Polygon2DEasyBuilder
    {
        private List<Vector2> _vertices = new();
        private Vector2 _lastVertex = Vector2.zero;

        public Polygon2DEasyBuilder WorldPoint(Vector2 point)
        {
            _vertices.Add(point);
            _lastVertex = point;
            return this;
        }

        public Polygon2DEasyBuilder NextPoint(Vector2 toNextPoint)
        {
            WorldPoint(_lastVertex + toNextPoint);
            return this;
        }

        public Polygon2D Build()
        {
            return new Polygon2D(_vertices.ToArray());
        }
    }
}
