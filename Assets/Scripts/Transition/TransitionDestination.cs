using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ŀ�������ö��
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
    /// ��ǰĿ������
    /// </summary>
    [Tooltip("��ǰĿ������")]
    public DestinationTag CurrentTag;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
