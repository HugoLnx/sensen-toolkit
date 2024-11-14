using System;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public readonly struct Box
    {
        public readonly Vector3 Right;
        public readonly Vector3 Up;
        public readonly Vector3 Forward;
        public readonly Vector3 HalfExtents;
        public readonly Vector3 Size;
        public readonly Vector3 WorldCenter;
        public readonly Vector3 HalfExtentsRight => Right * HalfExtents.x;
        public readonly Vector3 HalfExtentsUp => Up * HalfExtents.y;
        public readonly Vector3 HalfExtentsForward => Forward * HalfExtents.z;
        public readonly Vector3 SizeRight => Right * Size.x;
        public readonly Vector3 SizeUp => Up * Size.y;
        public readonly Vector3 SizeForward => Forward * Size.z;
        public readonly Vector3 WorldMin => WorldCenter - HalfExtentsRight - HalfExtentsUp - HalfExtentsForward;
        public readonly Vector3 WorldMax => WorldCenter + HalfExtentsRight + HalfExtentsUp + HalfExtentsForward;
        public readonly Quaternion Orientation => Quaternion.LookRotation(Forward, Up);
        public readonly Vector3 WorldHalfExtentsScaled(float x, float y, float z) => HalfExtentsRight * x + HalfExtentsUp * y + HalfExtentsForward * z;
        public readonly Vector3 WorldSizeScaled(float x, float y, float z) => SizeRight * x + SizeUp * y + SizeForward * z;

        public Box(Vector3 worldCenter, Vector3? forward = null, Vector3? up = null, Vector3? right = null, Vector3? extents = null, Vector3? size = null)
        {
            int directionValueCount = (forward.HasValue ? 1 : 0) + (up.HasValue ? 1 : 0) + (right.HasValue ? 1 : 0);
            if (directionValueCount != 2) throw new ArgumentException("Exactly 2 of Forward, Up, Right must be set");
            if (extents.HasValue && size.HasValue) throw new ArgumentException("Only one of Extents, Size must be set");
            if (!extents.HasValue && !size.HasValue) throw new ArgumentException("One of Extents, Size must be set");

            if (forward.HasValue && up.HasValue) right = Vector3.Cross(forward.Value, up.Value);
            if (forward.HasValue && right.HasValue) up = Vector3.Cross(right.Value, forward.Value);
            if (up.HasValue && right.HasValue) forward = Vector3.Cross(up.Value, right.Value);
            Right = right.Value;
            Up = up.Value;
            Forward = forward.Value;

            if (extents.HasValue) size = extents.Value * 2f;
            if (size.HasValue) extents = size.Value / 2f;
            HalfExtents = extents.Value;
            Size = size.Value;

            WorldCenter = worldCenter;
        }

        public Box(Vector3 origin, Vector3 forward, Vector3 up, Vector3 positiveGrow, Vector3 negativeGrow)
        {
            forward = forward.normalized;
            up = up.normalized;
            var right = Vector3.Cross(forward, up);
            Vector3 growDiff = positiveGrow - negativeGrow;
            Vector3 center = origin + (right * growDiff.x + up * growDiff.y + forward * growDiff.z) * 0.5f;
            Vector3 size = positiveGrow + negativeGrow;
            this = new Box(
                worldCenter: center,
                forward: forward,
                up: up,
                size: size
            );
        }

        public static Box FromBoxCollider(BoxCollider collider)
        {
            Vector3 worldCenter = collider.transform.TransformPoint(collider.center);
            Vector3 forward = collider.transform.forward;
            Vector3 up = collider.transform.up;
            var size = Vector3.Scale(collider.transform.localScale, collider.size);
            return new Box(
                worldCenter: worldCenter,
                forward: forward,
                up: up,
                size: size
            );
        }

        public void DebugDraw(Color? color = null, float? duration = null)
        {
            Vector3? prevPoint = null;
            foreach (Vector3 point in BuildDrawPoints())
            {
                if (prevPoint.HasValue)
                {
                    Debug.DrawLine(prevPoint.Value, point, color ?? Color.red, duration ?? 0.5f);
                }
                prevPoint = point;
            }
        }

        public void DebugDrawGizmos()
        {
            Gizmos.DrawLineList(BuildDrawPoints());
        }

        private Vector3[] BuildDrawPoints()
        {
            List<Vector3> points = new();

            for (int i = 0; i < 4; i++)
            {
                float a = i % 2;
                float b = i / 2;
                points.Add(WorldMin + WorldSizeScaled(a, b, 0));
                points.Add(WorldMin + WorldSizeScaled(a, b, 1));

                points.Add(WorldMin + WorldSizeScaled(a, 1, b));
                points.Add(WorldMin + WorldSizeScaled(a, 0, b));

                points.Add(WorldMin + WorldSizeScaled(1, a, b));
                points.Add(WorldMin + WorldSizeScaled(0, a, b));
            }

            return points.ToArray();
        }
    }
}
