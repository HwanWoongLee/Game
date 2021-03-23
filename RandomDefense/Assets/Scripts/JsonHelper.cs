using System;
using UnityEngine;
using System.Collections;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] combineData;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.combineData;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.combineData = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.combineData = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}
