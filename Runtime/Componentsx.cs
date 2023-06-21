using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public static class Componentsx
    {
        private const bool DEFAULT_SEARCH_CHILDREN = false;

        public static T EnsureComponent<T>(Component component,
            bool searchChildren = DEFAULT_SEARCH_CHILDREN)
        where T : UnityEngine.Component
        {
            if (component == null) return null;
            return EnsureComponent<T>(component.gameObject, searchChildren);
        }

        public static T EnsureComponent<T>(GameObject obj,
            bool searchChildren = DEFAULT_SEARCH_CHILDREN)
        where T : UnityEngine.Component
        {
            return SearchComponent<T>(obj, searchChildren) ?? obj.AddComponent<T>();
        }

        private static T SearchComponent<T>(GameObject obj, bool searchChildren) where T : Component
        {
            T component = searchChildren
                ? obj.GetComponentInChildren<T>(includeInactive: true)
                : obj.GetComponent<T>();
            return component == null ? null : component;
        }
    }
}
