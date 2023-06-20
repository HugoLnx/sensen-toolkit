using UnityEngine;

namespace SensenToolkit
{
    public static class Vector3ExtensionMethods
    {
        public static Vector2 XZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
        public static Vector3 X0Z(this Vector3 v3)
        {
            return new Vector3(v3.x, 0, v3.z);
        }
        public static Vector3 XYZ(this Vector3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }
        public static Vector2 XY(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector3 Clone(this Vector3 v3, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                x ?? v3.x,
                y ?? v3.y,
                z ?? v3.z
            );
        }
    }
}
