using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 人物基本属性
    /// </summary>
    public CharacterData characterData;

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


}
