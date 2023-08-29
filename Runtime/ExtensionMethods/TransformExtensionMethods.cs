using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SensenToolkit
{
    public static class TransformExtensionMethods
    {
        public static void DestroyAllChildren(this Transform transform, IEnumerable<Transform> except = null)
        {
            HashSet<int> exceptIds = except != null
                ? new HashSet<int>(except.Select(t => t.GetInstanceID()))
                : null;
            foreach (Transform child in transform)
            {
                if (exceptIds?.Contains(child.GetInstanceID()) == true)
                {
                    continue;
                }
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
