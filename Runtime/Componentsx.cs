using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public static class Componentsx
    {
        public static T EnsureComponent<T>(Component component)
        where T : UnityEngine.Component
        {
            if (component == null) return null;
            return EnsureComponent<T>(component.gameObject);
        }

        public static T EnsureComponent<T>(GameObject obj)
        where T : UnityEngine.Component
        {
            return obj.GetComponent<T>() ?? obj.AddComponent<T>();
        }
    }
}
