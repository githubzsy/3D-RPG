using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TransitionType
{
    SameScene, DifferentScene
}


public class TransitionPoint : MonoBehaviour
{
    /// <summary>
    /// 要传送到哪个场景
    /// </summary>
    [Tooltip("要传送到哪个场景")]
    public string DestinationSceneName;

    /// <summary>
    /// 目标场景类型
    /// </summary>
    [Tooltip("目标场景类型")]
    public TransitionType DestinationTransitionType;

    /// <summary>
    /// 传送到哪个点
    /// </summary>
    [Tooltip("传送到哪个点")]
    public DestinationTag DestinationTag;

    /// <summary>
    /// 能否被传说
    /// </summary>
    private bool canTrans;
    private void OnTriggerStay(Collider other)
    {
        canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canTrans = false;
    }
}
