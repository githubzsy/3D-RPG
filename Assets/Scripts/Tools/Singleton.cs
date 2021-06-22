using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��װ���������࣬���ͱ�����������
/// </summary>
/// <typeparam name="T">����ʵ������</typeparam>
public class Singleton<T>: MonoBehaviour where T:Singleton<T>
{
    private static T instance;

    /// <summary>
    /// ��ǰ����
    /// </summary>
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = (T) this;
    }

    /// <summary>
    /// �Ƿ��ʼ�����
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
