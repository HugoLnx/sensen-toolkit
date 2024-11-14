using UnityEngine;

namespace SensenToolkit
{
    public static class Gizmosx
    {
        public static readonly WithColor WithColorInstance = new();
        public static WithColor Color(Color color)
        {
            // TODO: Use a pool of WithColorInstance instead of a single instance
            // to avoid problem with multiple objects / coroutines using it
            return WithColorInstance.UseColor(color);
        }

        public static void DrawArrow(Vector2 from, Vector2 to, float extendBy = 1f)
        {
            Vector2 rawVector = to - from;
            to = from + (rawVector * extendBy);
            Vector2 arrowVector = to - from;
            Gizmos.DrawLine(from, from + arrowVector);
            Vector2 arrowHeadVector = -arrowVector * 0.1f;
            Gizmos.DrawLine(to, to + arrowHeadVector.RotateBy(30f));
            Gizmos.DrawLine(to, to + arrowHeadVector.RotateBy(-30));
        }

        public static void DebugDrawPointAsterist(Vector3 point, Color? color = null, float size = 0.2f, float duration = 0.5f)
        {
            color ??= UnityEngine.Color.green;
            Debug.DrawLine(point + Vector3.one * size, point - Vector3.one * size, color.Value, duration);
            Debug.DrawLine(point + new Vector3(1f, 1f, -1f) * size, point - new Vector3(1f, 1f, -1f) * size, color.Value, duration);
            Debug.DrawLine(point + new Vector3(1f, -1f, 1f) * size, point - new Vector3(1f, -1f, 1f) * size, color.Value, duration);
        }
    }
}
