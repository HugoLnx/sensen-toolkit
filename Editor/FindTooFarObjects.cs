using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SensenToolkit.Mathx;

namespace SensenToolkit.EditorToolkit
{
    public sealed class FindTooFarObjects : Editor
    {
        private const float MaxDistance = 500f;
        private const float MaxScale = 200f;
        private const float MaxRectDistance = 1200f;
        [MenuItem("Tools/Sensen/FindFarObjects")]
        public static void FindFarObjects() {
            List<GameObject> farObjs = new();
            List<GameObject> bigScaleObjs = new();
            List<GameObject> rectOverflowObjs = new();
            List<GameObject> unusualObjs = new();
            float maxX = 0f;
            float maxY = 0f;
            float maxZ = 0f;
            float maxRectX = 0f;
            float maxRectY = 0f;
            var allObjs = GameObject.FindObjectsOfType<GameObject>();
            for (var i = 0; i < allObjs.Length; i++) {
                Transform transform = allObjs[i].transform;
                RectTransform rect = transform as RectTransform;
                bool isRect = rect != null;
                if (isRect)
                {
                    maxRectX = Mathf.Max(maxRectX, Mathf.Abs(rect.rect.xMin));
                    maxRectX = Mathf.Max(maxRectX, Mathf.Abs(rect.rect.xMax));
                    maxRectY = Mathf.Max(maxRectY, Mathf.Abs(rect.rect.yMin));
                    maxRectY = Mathf.Max(maxRectY, Mathf.Abs(rect.rect.yMax));
                }
                else {
                    maxX = Mathf.Max(maxX, Mathf.Abs(transform.position.x));
                    maxY = Mathf.Max(maxY, Mathf.Abs(transform.position.y));
                    maxZ = Mathf.Max(maxZ, Mathf.Abs(transform.position.z));
                }
                if ((
                    !isRect && (
                        Mathf.Abs(transform.position.x) >= MaxDistance
                        || Mathf.Abs(transform.position.y) >= MaxDistance
                    ))
                    || Mathf.Abs(transform.position.z) >= MaxDistance
                )
                {
                    farObjs.Add(allObjs[i]);
                }

                if (
                    !Floatx.IsUsual(transform.position.x)
                    || !Floatx.IsUsual(transform.position.y)
                    || !Floatx.IsUsual(transform.position.z)
                    || !Floatx.IsUsual(transform.localScale.x)
                    || !Floatx.IsUsual(transform.localScale.y)
                    || !Floatx.IsUsual(transform.localScale.z)
                    || !Floatx.IsUsual(transform.rotation.x)
                    || !Floatx.IsUsual(transform.rotation.y)
                    || !Floatx.IsUsual(transform.rotation.z)
                    || (isRect && (
                        !Floatx.IsUsual(rect.rect.xMin)
                        || !Floatx.IsUsual(rect.rect.xMax)
                        || !Floatx.IsUsual(rect.rect.yMin)
                        || !Floatx.IsUsual(rect.rect.yMax)
                    ))
                )
                {
                    unusualObjs.Add(allObjs[i]);
                }

                if (
                    Mathf.Abs(transform.localScale.x) >= MaxScale
                    || Mathf.Abs(transform.localScale.y) >= MaxScale
                    || Mathf.Abs(transform.localScale.z) >= MaxScale
                )
                {
                    bigScaleObjs.Add(allObjs[i]);
                }

                if (isRect && (
                    Mathf.Abs(rect.rect.yMin) >= MaxRectDistance
                    || Mathf.Abs(rect.rect.yMax) >= MaxRectDistance
                    || Mathf.Abs(rect.rect.xMin) >= MaxRectDistance
                    || Mathf.Abs(rect.rect.xMax) >= MaxRectDistance
                ))
                {
                    rectOverflowObjs.Add(allObjs[i]);
                }
            }

            if (unusualObjs.Count > 0) {
                Debug.LogWarning($"Found {unusualObjs.Count} Unusual objects...");
                for (var i = 0; i < unusualObjs.Count; i++) {
                    string objData = "position:{unusualObjs[i].transform.position}"
                        + $"\nlocalScale:{unusualObjs[i].transform.localScale}"
                        + $"\nrotation:{unusualObjs[i].transform.rotation}";

                    if (unusualObjs[i].transform is RectTransform rect)
                    {
                        objData += $"\nrect:{rect.rect}";
                    }

                    Debug.Log($"Unusual Found: {unusualObjs[i].name}"
                    + $"\n{objData}"
                    + $"\nFullPath: {unusualObjs[i].transform.FullPath()}");
                }
            } else {
                Debug.Log("No Unusual objects");
            }

            if (farObjs.Count > 0) {
                Debug.LogWarning($"Found {farObjs.Count} Far objects...");
                for (var i = 0; i < farObjs.Count; i++) {
                    Debug.Log($"Far Object Found: {farObjs[i].name} at location {farObjs[i].transform.position}"
                    + $"\nFullPath: {farObjs[i].transform.FullPath()}");
                }
            } else {
                Debug.Log("No Far objects");
            }

            if (bigScaleObjs.Count > 0) {
                Debug.LogWarning($"Found {bigScaleObjs.Count} Big Scale objects...");
                for (var i = 0; i < bigScaleObjs.Count; i++) {
                    Debug.Log($"Big Scale Found: {bigScaleObjs[i].name} at scale {bigScaleObjs[i].transform.localScale}"
                    + $"\nFullPath: {bigScaleObjs[i].transform.FullPath()}");
                }
            } else {
                Debug.Log("No Big Scale objects");
            }

            if (rectOverflowObjs.Count > 0) {
                Debug.LogWarning($"Found {rectOverflowObjs.Count} Rect Overflow objects...");
                for (var i = 0; i < rectOverflowObjs.Count; i++) {
                    RectTransform rect = rectOverflowObjs[i].transform as RectTransform;
                    Debug.Log($"Rect Overflow Found: {rectOverflowObjs[i].name} at rect {rect.rect}"
                    + $"\nFullPath: {rectOverflowObjs[i].transform.FullPath()}");
                }
            } else {
                Debug.Log("No Rect Overflow objects");
            }

            Debug.Log($"Max Position: x:{maxX} y:{maxY} z:{maxZ}");
            Debug.Log($"Max Rect: x:{maxRectX} y:{maxRectY}");
        }
    }
}
