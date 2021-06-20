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
    /// ������ֵ
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
    /// �ƶ�����������
    /// </summary>
    /// <param name="obj"></param>
    private void EventAttack(GameObject obj)
    {
        if (obj != null)
        {
            attackTarget = obj;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
             c = StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
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
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    public void MoveToTarget(Vector3 target)
    {
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

    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats);
    }
}
