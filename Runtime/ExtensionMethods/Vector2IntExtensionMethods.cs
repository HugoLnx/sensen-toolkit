using UnityEngine;

namespace SensenToolkit
{
    public static class Vector2IntExtensionMethods
    {
        public static Vector3Int X0Y(this Vector2Int v2)
        {
            return new Vector3Int(v2.x, 0, v2.y);
        }
        public static Vector3Int XY0(this Vector2Int v2)
        {
            return new Vector3Int(v2.x, v2.y, 0);
        }

        public static Vector2Int PerpendicularAntiClockwise(this Vector2Int v)
        {
            return new Vector2Int(-v.y, v.x);
        }

        public static Vector2Int PerpendicularClockwise(this Vector2Int v)
        {
            return -PerpendicularAntiClockwise(v);
        }

        public static Vector2Int With(this Vector2Int vec, int? x = null, int? y = null)
        {
            return new Vector2Int(
                x: x ?? vec.x,
                y: y ?? vec.y
            );
        }

        public static Vector2 AsVector2Float(this Vector2Int vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}
