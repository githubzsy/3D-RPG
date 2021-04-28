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

    public float lookAtTime;

    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;

    /// <summary>
    /// 起始坐标
    /// </summary>
    private Vector3 guardPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        defaultSpeed = agent.speed;
        guardPosition = transform.position;
        remainLookAtTime = lookAtTime;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.Gaurd;
        }
        else
        {
            enemyStates = EnemyStates.Patrol;
            GetNewWayPoint();
        }
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
                isChase=false;
                agent.speed = defaultSpeed * 0.5f;
                // 如果已经接近导航点则停止行走并设定一个新的导航点
                if(Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
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
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.Gaurd;
                    }
                    else
                    {
                        enemyStates = EnemyStates.Patrol;
                    }
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

    /// <summary>
    /// 设定一个新的导航点
    /// </summary>
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        // 在初始坐标的周围巡逻
        Vector3 randomPoint = new Vector3(guardPosition.x + randomX, transform.position.y, guardPosition.z + randomZ);
        // 判断randomPoint是否为1，也就是walkable
        wayPoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1) ? hit.position : transform.position;
    }

    /// <summary>
    /// 选中当前对象时绘制辅助线
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 以当前对象为圆心，sightRadius为半径绘制
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}

