using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 封装单例泛型类，泛型必须是其子类
/// </summary>
/// <typeparam name="T">子类实际类型</typeparam>
public class Singleton<T>: MonoBehaviour where T:Singleton<T>
{
    private static T instance;

    /// <summary>
    /// 当前单例
    /// </summary>
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = (T) this;
    }

    /// <summary>
    /// 是否初始化完成
    /// </summary>
    public static bool IsInitialized => instance != null;

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
