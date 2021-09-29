using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    [Tooltip("������")]
    public float KickForce = 200;

    /// <summary>
    /// ʯͷԤ����
    /// </summary>
    [Tooltip("ʯͷԤ����")]
    public GameObject rockPrefab;


    [Tooltip("�ֲ�λ��")]
    public Transform handPosition;

    /// <summary>
    /// Animation Event
    /// </summary>
    public void KickOff()
    {
        if (attackTarget != null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            var direction = (attackTarget.transform.position - transform.position).normalized;

            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            if (this.characterStats.isCritical)
            {
                // ����
                targetStats.GetComponent<NavMeshAgent>().velocity = direction * KickForce;
            }

            // ����
            // targetStats.GetComponent<Animator>().SetTrigger("Dizzy");
            
            targetStats.TakeDamage(characterStats);
        }
    }

    /// <summary>
    /// Animation Event ��ʯͷ
    /// </summary>
    public void ThrowRock()
    {
        if (attackTarget == null)
        {
            // ����ʯͷ�Ķ����Ѿ������ˣ��������������ս���������ҵ������Ϊ����Ŀ��
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }
        var rock = Instantiate(rockPrefab, handPosition.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        
    }
}
