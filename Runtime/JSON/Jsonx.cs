using UnityEngine;

namespace SensenToolkit
{
    public static class JSONx
    {
        public static T FromJson<T>(string json)
        {
            return ((JSONxDataWrapper<T>) JsonUtility.FromJson<JSONxDataWrapper<T>>(json)).Data;
        }

        public static string ToJson<T>(T data)
        {
            JSONxDataWrapper<T> wrapped = new() { Data = data};
            return JsonUtility.ToJson(wrapped);
        }
    }
}
