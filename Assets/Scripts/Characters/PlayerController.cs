using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    private GameObject attackTarget;

    /// <summary>
    /// ������ֵ
    /// </summary>
    private CharacterStats characterStats;

    private float lastAttackTime;

    private bool isDead;

    /// <summary>
    /// ԭʼ��ֹͣ����
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
    /// �ƶ�����������
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
        // �޸Ĺ������������﹥��������ͬ
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        // ������������﹥������
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            // ������һ���ж�
            yield return null;
        }

        agent.isStopped = true;
        // ����
        if (lastAttackTime < 0)
        {
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            // ���ù�����ȴʱ��
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
    /// �ƶ���Ŀ��
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Vector3 target)
    {
        if (isDead) return;
        if (c != null)
        {
            StopCoroutine(c);
        }
        // ��ֹͣ����ָ�ΪĬ��ֵ
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
                // ��ʯͷһ������ǰ����
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
