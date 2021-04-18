using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    private bool isWalk;

    private bool isChase;

    private bool isFollow;

    private EnemyStates enemyStates;

    [Header("Basic Settings")]
    public float sightRadius;

    /// <summary>
    /// 是否为站桩敌人
    /// </summary>
    public bool isGuard;

    /// <summary>
    /// 默认速度
    /// </summary>
    private float defaultSpeed;

    private GameObject attackTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        defaultSpeed = agent.speed;
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }

    void SwitchAnimation()
    {
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
    }

    void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.Chase;
        }

        switch (enemyStates)
        {
            
            case EnemyStates.Gaurd:
                break;
            case EnemyStates.Patrol:
                
                break;
            case EnemyStates.Chase:
                // TODO:追player

                isWalk = false;
                isChase = true;

                agent.speed = defaultSpeed;
                if (!FoundPlayer())
                {
                    // TODO:拉脱回到上一个状态
                    isFollow = false;
                    agent.destination = transform.position;
                }
                else
                {
                    isFollow = true;
                    // 找到玩家时，设定怪物导航目标
                    agent.destination = attackTarget.transform.position;
                }

                // TODO:在攻击范围内则攻击

                // TODO:配合攻击动画
                break;
            case EnemyStates.Dead:
                break;
        }
    }

    /// <summary>
    /// 是否找到Player
    /// </summary>
    /// <returns></returns>
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        var targetCollider= colliders.FirstOrDefault(a => a.CompareTag("Player"));
        // 找到攻击目标
        if (targetCollider != null)
        {
            attackTarget = targetCollider.gameObject;
            return true;
        }

        attackTarget = null;
        return false;
    }
}

