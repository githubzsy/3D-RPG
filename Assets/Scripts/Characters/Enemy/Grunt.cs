using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    [Tooltip("������")]
    public float KickForce = 100;

    public void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);

            // ���ķ��򣺴ӵ���ָ�����= ���λ��- ����λ��
            Vector3 direction = attackTarget.transform.position - transform.position;
            // ��һ��
            direction.Normalize();
            var navMeshAgent = attackTarget.GetComponent<NavMeshAgent>();
            // �������ƶ�����
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = direction * KickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
