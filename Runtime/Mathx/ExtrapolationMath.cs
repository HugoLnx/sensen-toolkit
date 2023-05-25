using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensenToolkit.Mathx;

namespace Rockskiing
{
    public static class ExtrapolationMath
    {
        public static Vector2 ExtrapolatePointHorizontally(Vector2 from, Vector2 through, float horizontalDistance, float maxAngle)
        {
            Vector2 direction = Math2Dx.ClampVector2Angle(
                vector: through - from,
                min: -maxAngle,
                max: maxAngle,
                includeReflection: true
            );
            //Vector2 direction = through - from;
            return through + Math2Dx.ScaleToHorizontalLength(direction, horizontalDistance);
        }

        public static Vector2 ExtrapolatePoint(Vector2 from, Vector2 through, float distance)
        {
            Vector2 direction = (through - from).normalized;
            return through + (direction * distance);
        }
    }
}
