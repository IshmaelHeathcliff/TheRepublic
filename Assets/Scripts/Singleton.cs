using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    T _instance;

    public T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
        }
    }
}