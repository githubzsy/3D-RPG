using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }

    public RockStates State;

    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;

    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        State = RockStates.HitPlayer;
        FlyToPlayer();
    }

    Vector3 direction;
    /// <summary>
    /// 飞向玩家
    /// </summary>
    public void FlyToPlayer()
    {
        if (target == null)
        {
            // 当石头生成出来，但是玩家已经脱离战斗，还是应该飞向玩家
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        FlyToTarget(target);
    }

    /// <summary>
    /// 飞向目标
    /// </summary>
    /// <param name="target"></param>
    public void FlyToTarget(GameObject target)
    {
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var relativeVelocity = rb.GetRelativePointVelocity(collision.transform.position);
        // 若相对于目标的magnitude小于3，则认为没有攻击性
        if (relativeVelocity.magnitude < 3)
        {
            State = RockStates.HitNothing;
            return;
        }

        var velocity = relativeVelocity * rb.mass / 10;
        switch (State)
        {
            // 攻击到玩家
            case RockStates.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = velocity;
                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(10, false,null);
                    State = RockStates.HitNothing;
                }
                break;
            // 攻击到敌人
            case RockStates.HitEnemy:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = velocity / 2;
                    collision.gameObject.GetComponent<Animator>().SetTrigger("GetHit");
                    var player = FindObjectOfType<PlayerController>().GetComponent<CharacterStats>();
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(30, false, player);
                    State = RockStates.HitNothing;
                }
                break;
        }
    }
}
