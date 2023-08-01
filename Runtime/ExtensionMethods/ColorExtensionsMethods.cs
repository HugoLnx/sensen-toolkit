using UnityEngine;

namespace SensenToolkit
{
    public static class ColorExtensionMethods
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}
