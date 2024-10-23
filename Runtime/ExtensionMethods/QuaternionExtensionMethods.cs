using UnityEngine;

namespace SensenToolkit
{
    public static class QuaternionExtensionMethods
    {
        public static Vector3 AsVector3(this Quaternion q) => new(q.x, q.y, q.z);
        public static Vector4 AsVector4(this Quaternion q) => new(q.x, q.y, q.z, q.w);
    }
}
