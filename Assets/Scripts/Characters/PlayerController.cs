using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    private GameObject attackTarget;

    /// <summary>
    /// 人物数值
    /// </summary>
    private CharacterStats characterStats;

    private float lastAttackTime;

    private Coroutine c;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    /// <summary>
    /// 移动并攻击敌人
    /// </summary>
    /// <param name="obj"></param>
    private void EventAttack(GameObject obj)
    {
        if (obj != null)
        {
            attackTarget = obj;
             c = StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        transform.LookAt(attackTarget.transform);
        // 若距离大于人物攻击距离
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            // 进行下一次判断
            yield return null;
        }

        agent.isStopped = true;
        // 攻击
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("Attack");
            // 重置攻击冷却时间
            lastAttackTime = 0.5f;
        }
    }



    void Update()
    {
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    public void MoveToTarget(Vector3 target)
    {
        Debug.Log("朝目标移动");
        if (c != null)
        {
            StopCoroutine(c);
        }
        agent.isStopped = false;
        agent.destination = target;
    }

    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }
}
