using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    [Tooltip("击飞力")]
    public float KickForce = 200;

    /// <summary>
    /// 石头预制体
    /// </summary>
    [Tooltip("石头预制体")]
    public GameObject rockPrefab;


    [Tooltip("手部位置")]
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
                // 击飞
                targetStats.GetComponent<NavMeshAgent>().velocity = direction * KickForce;
            }

            // 击晕
            // targetStats.GetComponent<Animator>().SetTrigger("Dizzy");
            
            targetStats.TakeDamage(characterStats);
        }
    }

    /// <summary>
    /// Animation Event 扔石头
    /// </summary>
    public void ThrowRock()
    {
        if (attackTarget == null)
        {
            // 当丢石头的动作已经出来了，但是玩家脱离了战斗，重新找到玩家作为攻击目标
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }
        var rock = Instantiate(rockPrefab, handPosition.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        
    }
}
