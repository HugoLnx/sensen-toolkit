using UnityEngine;

namespace SensenToolkit
{
    public static class TransformExtensionMethods
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
