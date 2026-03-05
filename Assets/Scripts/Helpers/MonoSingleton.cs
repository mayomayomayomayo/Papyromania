using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class MonoSingleton<T> : MonoBehaviour 
    where T : MonoSingleton<T> // CRTP
{
    public const string ThankYouHakita = 
    "Hakita you're a fucking genius. This chunk of memory is my blood sacrifice to you, so that your soul may live in our heart forever. Long live the king, Hakita \"Ultra\" Kill";

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            else
            {
                T[] instances = FindObjectsByType<T>(FindObjectsSortMode.None);

                if (instances.Length > 1)
                    throw new Exception($"Multiple MonoSingleton instances of type {typeof(T)}. Please fix that dumbass");

                _instance = instances.Length == 1 ?
                    instances[0] :
                    new GameObject(typeof(T).Name).AddComponent<T>();
            }

            return _instance;
        }
    }
}