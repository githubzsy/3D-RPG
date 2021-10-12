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
    /// Ҫ���͵��ĸ�����
    /// </summary>
    [Tooltip("Ҫ���͵��ĸ�����")]
    public string DestinationSceneName;

    /// <summary>
    /// Ŀ�곡������
    /// </summary>
    [Tooltip("Ŀ�곡������")]
    public TransitionType DestinationTransitionType;

    /// <summary>
    /// ���͵��ĸ���
    /// </summary>
    [Tooltip("���͵��ĸ���")]
    public DestinationTag DestinationTag;

    /// <summary>
    /// �ܷ񱻴�˵
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
