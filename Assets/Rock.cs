using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }

    public RockStates State;

    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;

    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        State = RockStates.HitPlayer;
        FlyToPlayer();
    }

    Vector3 direction;
    /// <summary>
    /// �������
    /// </summary>
    public void FlyToPlayer()
    {
        if (target == null)
        {
            // ��ʯͷ���ɳ�������������Ѿ�����ս��������Ӧ�÷������
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        FlyToTarget(target);
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    /// <param name="target"></param>
    public void FlyToTarget(GameObject target)
    {
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var relativeVelocity = rb.GetRelativePointVelocity(collision.transform.position);
        // �������Ŀ���magnitudeС��3������Ϊû�й�����
        if (relativeVelocity.magnitude < 3)
        {
            State = RockStates.HitNothing;
            return;
        }

        var velocity = relativeVelocity * rb.mass / 10;
        switch (State)
        {
            // ���������
            case RockStates.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = velocity;
                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(10, false);
                    State = RockStates.HitNothing;
                }
                break;
            // ����������
            case RockStates.HitEnemy:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = velocity / 2;
                    collision.gameObject.GetComponent<Animator>().SetTrigger("GetHit");
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(30, false);
                    State = RockStates.HitNothing;
                }
                break;
        }
    }
}
