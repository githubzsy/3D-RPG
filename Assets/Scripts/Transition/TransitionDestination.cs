using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 目标点类型枚举
/// </summary>
public enum DestinationTag
{
    ENTER,
    A,
    B,
    C
}


public class TransitionDestination : MonoBehaviour
{
    /// <summary>
    /// 当前目标类型
    /// </summary>
    [Tooltip("当前目标类型")]
    public DestinationTag CurrentTag;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
