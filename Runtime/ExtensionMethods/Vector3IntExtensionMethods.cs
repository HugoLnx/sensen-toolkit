using UnityEngine;

namespace SensenToolkit
{
    public static class Vector3IntExtensionMethods
    {
        public static Vector2Int XZ(this Vector3Int v3)
        {
            return new Vector2Int(v3.x, v3.z);
        }
        public static Vector3Int X0Z(this Vector3Int v3)
        {
            return new Vector3Int(v3.x, 0, v3.z);
        }
        public static Vector3Int XYZ(this Vector3Int v3)
        {
            return new Vector3Int(v3.x, v3.y, v3.z);
        }
        public static Vector3Int XZY(this Vector3Int v3)
        {
            return new Vector3Int(v3.x, v3.z, v3.y);
        }
        public static Vector2Int XY(this Vector3Int v3)
        {
            return new Vector2Int(v3.x, v3.y);
        }

        public static Vector3 AsVector3Float(this Vector3Int v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static Vector3Int Clone(this Vector3Int v3, int? x = null, int? y = null, int? z = null)
        {
            return new Vector3Int(
                x ?? v3.x,
                y ?? v3.y,
                z ?? v3.z
            );
        }
    }
}
