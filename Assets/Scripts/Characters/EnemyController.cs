using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private NavMeshAgent agent;

    private Animator animator;

    private Collider collider;

    private bool isWalk;

    private bool isChase;

    /// <summary>
    /// 是否追赶玩家
    /// </summary>
    private bool isFollow;

    private bool isDead;

    private bool playerDead;

    private EnemyStates enemyStates;

    private CharacterStats characterStats;

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

    /// <summary>
    /// 当前攻击目标
    /// </summary>
    protected GameObject attackTarget;

    public float lookAtTime;

    private float remainLookAtTime;
    
    /// <summary>
    /// 攻击计时器(为0时才可以进行下一次攻击)
    /// </summary>
    private float attackTimer;

    /// <summary>
    /// 初始旋转角度
    /// </summary>
    private Quaternion guardRotation;

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
        characterStats = GetComponent<CharacterStats>();
        collider = GetComponent<Collider>();
        defaultSpeed = agent.speed;
        // 原始角度
        guardRotation = transform.rotation;
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

        // FIXME:场景切换后修改掉
        GameManager.Instance.AddObserver(this);
    }

    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            attackTimer -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Critical",characterStats.isCritical);
        animator.SetBool("Death",isDead);
    }

    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.Dead;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.Chase;
        }

        switch (enemyStates)
        {
            // 站桩怪
            case EnemyStates.Gaurd:
                isChase = false;
                if (transform.position != guardPosition)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPosition;

                    // 判断是否到达目的地
                    if (Vector3.SqrMagnitude(guardPosition - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        // 旋转至默认角度
                        transform.rotation=Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
                    }
                }
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
                isWalk = false;
                isChase = true;

                agent.speed = defaultSpeed;
                if (!FoundPlayer())
                {
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
                    agent.isStopped = false;
                    // 找到玩家时，设定怪物导航目标
                    agent.destination = attackTarget.transform.position;
                }

                // 在攻击范围内则攻击
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    // 若小于0
                    if (attackTimer < 0)
                    {
                        // 重置定时器
                        attackTimer = characterStats.attackData.coolDown;

                        // 暴击判断
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                        Attack();
                    }
                }
                break;
            case EnemyStates.Dead:
                // 关闭碰撞体，这样玩家就不能点击了
                collider.enabled = false;
                // 关闭导航
                // agent.enabled = false;
                // 将半径设置为0，修复调用动画时找不到agent的bug
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            animator.SetTrigger("Attack");
        }

        if (TargetInSkillRange())
        {
            animator.SetTrigger("Skill");
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
    /// 目标是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                   characterStats.attackData.attackRange;
        }

        return false;
    }

    /// <summary>
    /// 目标是否在技能范围内
    /// </summary>
    /// <returns></returns>
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                   characterStats.attackData.skillRange;
        }

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

    void Hit()
    {
        if (attackTarget != null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats);
        }
    }

    /// <summary>
    /// Player死的时候
    /// </summary>
    public void EndNotify()
    {
        playerDead = true;
        animator.SetBool("Win",true);
        isChase = false;
        isWalk = false;
        attackTarget = null;

    }

    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}

    void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
    }
}

