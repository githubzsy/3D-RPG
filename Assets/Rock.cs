using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;

    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlyToTarget();
    }

    public void FlyToTarget()
    {
        if (target == null)
        {
            // ��ʯͷ���ɳ�������������Ѿ�����ս��������Ӧ�÷������
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        var direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
