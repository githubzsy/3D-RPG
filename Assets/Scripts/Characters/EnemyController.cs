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
                // TODO:׷player

                isWalk = false;
                isChase = true;

                agent.speed = defaultSpeed;
                if (!FoundPlayer())
                {
                    // TODO:���ѻص���һ��״̬
                    isFollow = false;
                    agent.destination = transform.position;
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
}

