using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    [Tooltip("击飞力")]
    public float KickForce = 100;

    public void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);

            // 力的方向：从敌人指向玩家= 玩家位置- 敌人位置
            Vector3 direction = attackTarget.transform.position - transform.position;
            // 归一化
            direction.Normalize();
            var navMeshAgent = attackTarget.GetComponent<NavMeshAgent>();
            // 打断玩家移动动作
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = direction * KickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
