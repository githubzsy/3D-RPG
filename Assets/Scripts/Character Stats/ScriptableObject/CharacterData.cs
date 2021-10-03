using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName = "Character Stats/Data")]
public class CharacterData : ScriptableObject
{
    #region 属性相关
    /// <summary>
    /// 最大HP
    /// </summary>
    [Header("Stats Info")]
    [Tooltip("最大HP")]
    public int maxHealth;

    /// <summary>
    /// 当前HP
    /// </summary>
    [Tooltip("当前HP")]
    public int currentHealth;

    /// <summary>
    /// 基础防御
    /// </summary>
    [Tooltip("基础防御")]
    public int baseDefense;

    /// <summary>
    /// 当前防御
    /// </summary>
    [Tooltip("当前防御")]
    public int currentDefense;
    #endregion

    #region 经验相关
    /// <summary>
    /// 怪物经验呢
    /// </summary>
    [Header("Kill")]
    [Tooltip("怪物经验")]
    public int killPoint;

    /// <summary>
    /// 当前等级
    /// </summary>
    [Tooltip("当前等级")]
    public int currentLevel;

    /// <summary>
    /// 当前经验值
    /// </summary>
    [Tooltip("当前经验值")]
    public int currentExp;

    /// <summary>
    /// 升级点经验
    /// </summary>
    [Tooltip("升级点经验")]
    public int baseExp;

    /// <summary>
    /// 升级经验倍率
    /// </summary>
    [Tooltip("当前升级需要经验值倍率")]
    public float levelBuff = 1.2f;
    #endregion
    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        baseExp = (int)(baseExp * levelBuff);
        maxHealth += 50;
        currentHealth = maxHealth;
        baseDefense += 2;
        currentDefense += 2;
        currentLevel++;

        Debug.Log("Level Up");
    }
}
