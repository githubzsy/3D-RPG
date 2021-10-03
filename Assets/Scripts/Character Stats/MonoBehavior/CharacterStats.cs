using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 被攻击时的事件，前一个参数是当前血量，后一个参数是总血量
    /// </summary>
    public event Action<int, int> UpdateHealthBarOnTakeDamage;

    /// <summary>
    /// 人物属性模板
    /// </summary>
    public CharacterData templateData;

    /// <summary>
    /// 人物基本属性
    /// </summary>
    private CharacterData characterData;

    /// <summary>
    /// 人物攻击属性
    /// </summary>
    public AttackData attackData;

    /// <summary>
    /// 是否暴击了
    /// </summary>
    [HideInInspector]
    public bool isCritical;

    #region 读取characterData里面内容
    public int MaxHealth
    {
        get => characterData != null ? characterData.maxHealth : 0;
        set => characterData.maxHealth = value;
    }

    public int CurrentHealth
    {
        get => characterData != null ? characterData.currentHealth : 0;
        set => characterData.currentHealth = value;
    }

    public int BaseDefense
    {
        get => characterData != null ? characterData.baseDefense : 0;
        set => characterData.baseDefense = value;
    }

    public int CurrentDefense
    {
        get => characterData != null ? characterData.currentDefense : 0;
        set => characterData.currentDefense = value;
    }


    #endregion

    #region 人物战斗数值
    /// <summary>
    /// 承受伤害
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void TakeDamage(CharacterStats attacker)
    {
        // 最低伤害为0
        int damage = Mathf.Max(attacker.CurrentDamage() - this.CurrentDefense,0);
        TakeDamage(damage, attacker.isCritical);
    }

    /// <summary>
    /// 承受伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="isCritical">是否为暴击伤害</param>
    public void TakeDamage(int damage,bool isCritical)
    {
        // 保证最小血量不会到负数
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        // 被暴击时出现被击动作
        if (isCritical)
        {
            this.GetComponent<Animator>().SetTrigger("GetHit");
        }

        UpdateHealthBarOnTakeDamage?.Invoke(CurrentHealth, MaxHealth);
    }

    /// <summary>
    /// 计算实时伤害值
    /// </summary>
    /// <returns></returns>
    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);

        // 计算暴击伤害
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int) coreDamage;
    }

    #endregion

    void Awake()
    {
        // 初始化属性模板
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }
}
