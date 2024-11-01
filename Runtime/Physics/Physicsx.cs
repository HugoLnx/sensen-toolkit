using System;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public static class Physicsx
    {
        private static Collider[] s_collidersBuffer = new Collider[64];

        public static bool RaycastWithOverlap(
            Ray ray,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            HashSet<GameObject> ignoreOverlapOnAll = null,
            GameObject ignoreOverlapOnObj = null
        )
        {
            return RaycastWithOverlap(ray, out _, maxDistance, layerMask, queryTriggerInteraction, ignoreOverlapOnAll, ignoreOverlapOnObj);
        }
        public static bool RaycastWithOverlap(
            Ray ray,
            out RaycastHitx hitInfo,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            HashSet<GameObject> ignoreOverlapOnAll = null,
            GameObject ignoreOverlapOnObj = null
        )
        {
            return RaycastWithOverlap(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction, ignoreOverlapOnAll, ignoreOverlapOnObj);
        }
        public static bool RaycastWithOverlap(
            Vector3 origin,
            Vector3 direction,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            HashSet<GameObject> ignoreOverlapOnAll = null,
            GameObject ignoreOverlapOnObj = null
        )
        {
            return RaycastWithOverlap(origin, direction, out _, maxDistance, layerMask, queryTriggerInteraction, ignoreOverlapOnAll, ignoreOverlapOnObj);
        }

        public static bool RaycastWithOverlap(
            Vector3 origin,
            Vector3 direction,
            out RaycastHitx hitInfo,
            float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
            HashSet<GameObject> ignoreOverlapOnAll = null,
            GameObject ignoreOverlapOnObj = null
        )
        {
            hitInfo = default;

            int count = Physics.OverlapSphereNonAlloc(
                position: origin,
                radius: 0.01f,
                results: s_collidersBuffer,
                layerMask: layerMask,
                queryTriggerInteraction: queryTriggerInteraction
            );
            Collider overlaped = null;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = s_collidersBuffer[i].gameObject;
                if (ignoreOverlapOnAll != null && ignoreOverlapOnAll.Contains(obj)) continue;
                if (ignoreOverlapOnObj != null && obj == ignoreOverlapOnObj) continue;
                overlaped = s_collidersBuffer[i];
                break;
            }
            if (overlaped != null)
            {
                hitInfo.point = origin;
                hitInfo.normal = direction.normalized;
                hitInfo.distance = 0f;
                hitInfo.collider = overlaped;
                return true;
            }
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

            return false;
        }

        public static HashSet<GameObject> FindSelfColliderObjects(GameObject gameObject)
        {
            HashSet<GameObject> objs = new();
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>(includeInactive: true);
            foreach (Collider collider in colliders)
            {
                objs.Add(collider.gameObject);
            }
            return objs;
        }
    }
}
