using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    /// <summary>
    /// 攻击目标
    /// </summary>
    private GameObject attackTarget;

    /// <summary>
    /// 人物数值
    /// </summary>
    private CharacterStats characterStats;

    private float lastAttackTime;

    private bool isDead;

    /// <summary>
    /// 原始的停止距离
    /// </summary>
    private float stopDistance;

    private Coroutine c;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
    }

    void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RegisterPlayer(characterStats);
    }

    /// <summary>
    /// 移动并攻击敌人
    /// </summary>
    /// <param name="obj"></param>
    private void EventAttack(GameObject obj)
    {
        if (isDead) return;
        if (obj != null)
        {
            agent.isStopped = false;
            attackTarget = obj;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
            if (c != null)
            {
                StopCoroutine(c);
            }
            c = StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        if (isDead) yield break;
        // 修改攻击距离与人物攻击距离相同
        agent.stoppingDistance = characterStats.attackData.attackRange;
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
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            // 重置攻击冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }



    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            agent.enabled = false;
            attackTarget = null;
            GameManager.Instance.NotifyObservers();
        }

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;

    }
    
    /// <summary>
    /// 移动到目标
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Vector3 target)
    {
        if (isDead) return;
        if (c != null)
        {
            StopCoroutine(c);
        }
        // 将停止距离恢复为默认值
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;

    }

    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Death",isDead);
    }

    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            var rock = attackTarget.GetComponent<Rock>();
            if (rock != null)
            {
                rock.State = Rock.RockStates.HitEnemy;
                // 给石头一个面向前的力
                rock.GetComponent<Rigidbody>().AddForce(transform.forward * characterStats.attackData.maxDamage, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats);
        }
    }
}
