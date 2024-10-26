using UnityEngine;

namespace SensenToolkit
{
    public static class Physicsx
    {
        private static Collider[] s_collidersBuffer = new Collider[1];
        public static bool RaycastAndOverlap(
            Ray ray,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        )
        {
            return RaycastAndOverlap(ray, out _, maxDistance, layerMask, queryTriggerInteraction);
        }
        public static bool RaycastAndOverlap(
            Ray ray,
            out RaycastHitx hitInfo,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        )
        {
            return RaycastAndOverlap(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }
        public static bool RaycastAndOverlap(
            Vector3 origin,
            Vector3 direction,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        )
        {
            return RaycastAndOverlap(origin, direction, out _, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static bool RaycastAndOverlap(
            Vector3 origin,
            Vector3 direction,
            out RaycastHitx hitInfo,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        )
        {
            hitInfo = default;
            if (Physics.Raycast(
                origin,
                direction,
                out RaycastHit hitInfoX,
                maxDistance,
                layerMask,
                queryTriggerInteraction
            ))
            {
                hitInfo = RaycastHitx.FromRaycastHit(hitInfoX);
                return true;
            }
            int count = Physics.OverlapSphereNonAlloc(origin, 0.1f, s_collidersBuffer, layerMask);
            if (count == 0) return false;

            hitInfo.point = origin;
            hitInfo.normal = direction.normalized;
            hitInfo.distance = 0f;
            hitInfo.collider = s_collidersBuffer[0];
            return true;
        }
    }
}
