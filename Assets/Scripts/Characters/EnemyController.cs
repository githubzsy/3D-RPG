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
    /// �Ƿ�Ϊվ׮����
    /// </summary>
    public bool isGuard;

    /// <summary>
    /// Ĭ���ٶ�
    /// </summary>
    private float defaultSpeed;

    private GameObject attackTarget;

    public float lookAtTime;

    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;

    /// <summary>
    /// ��ʼ����
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
                // ����Ѿ��ӽ���������ֹͣ���߲��趨һ���µĵ�����
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
                // TODO:׷player

                isWalk = false;
                isChase = true;

                agent.speed = defaultSpeed;
                if (!FoundPlayer())
                {
                    // TODO:���ѻص���һ��״̬
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
                    // �ҵ����ʱ���趨���ﵼ��Ŀ��
                    agent.destination = attackTarget.transform.position;
                }

                // TODO:�ڹ�����Χ���򹥻�

                // TODO:��Ϲ�������
                break;
            case EnemyStates.Dead:
                break;
        }
    }

    /// <summary>
    /// �Ƿ��ҵ�Player
    /// </summary>
    /// <returns></returns>
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        var targetCollider= colliders.FirstOrDefault(a => a.CompareTag("Player"));
        // �ҵ�����Ŀ��
        if (targetCollider != null)
        {
            attackTarget = targetCollider.gameObject;
            return true;
        }

        attackTarget = null;
        return false;
    }

    /// <summary>
    /// �趨һ���µĵ�����
    /// </summary>
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        // �ڳ�ʼ�������ΧѲ��
        Vector3 randomPoint = new Vector3(guardPosition.x + randomX, transform.position.y, guardPosition.z + randomZ);
        // �ж�randomPoint�Ƿ�Ϊ1��Ҳ����walkable
        wayPoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1) ? hit.position : transform.position;
    }

    /// <summary>
    /// ѡ�е�ǰ����ʱ���Ƹ�����
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // �Ե�ǰ����ΪԲ�ģ�sightRadiusΪ�뾶����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}

