using UnityEngine;

namespace SensenToolkit
{
    public static class Vector4ExtensionMethods
    {
        public static Vector3 XYZ(this Vector4 v4) => v4;
        public static Vector3 AsVector3(this Vector4 v4) => v4;
        public static Quaternion AsQuaternion(this Vector4 v4) => new(v4.x, v4.y, v4.z, v4.w);

        public static Vector4 SetOnly(this Vector4 v4, float? x = null, float? y = null, float? z = null, float? w = null)
        {
            v4.Set(
                x ?? v4.x,
                y ?? v4.y,
                z ?? v4.z,
                w ?? v4.w
            );
            return v4;
        }

        public static Vector4 With(this Vector4 v4, float? x = null, float? y = null, float? z = null, float? w = null)
        {
            return new Vector4(
                x ?? v4.x,
                y ?? v4.y,
                z ?? v4.z,
                w ?? v4.w
            );
        }
    }
}
