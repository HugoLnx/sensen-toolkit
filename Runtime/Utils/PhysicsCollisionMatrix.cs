using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public static class PhysicsCollisionMatrix
    {
        private static Dictionary<int, int> s_masksByLayer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            s_masksByLayer = new Dictionary<int, int>();
            for (int i = 0; i < 32; i++)
            {
                int mask = 0;
                for (int j = 0; j < 32; j++)
                {
                    if (!Physics.GetIgnoreLayerCollision(i, j))
                    {
                        mask |= 1 << j;
                    }
                }
                s_masksByLayer.Add(i, mask);
            }
        }

        public static int MaskForWhatCollidesWith(int layer)
        {
            return s_masksByLayer[layer];
        }

        public static int MaskForWhatDoesntCollideWith(int layer)
        {
            return ~s_masksByLayer[layer];
        }
    }
}
