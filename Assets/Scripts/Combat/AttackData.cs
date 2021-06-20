using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Attack Data")]
public class AttackData : ScriptableObject
{
    public float attackRange;

    public float skillRange;

    public float coolDown;

    public int minDamage;

    public int maxDamage;

    /// <summary>
    /// ±©»÷±¶ÂÊ
    /// </summary>
    public float criticalMultiplier;

    /// <summary>
    /// ±©»÷ÂÊ
    /// </summary>
    public float criticalChance;
}
