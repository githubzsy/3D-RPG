using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// �����������
    /// </summary>
    public CharacterData characterData;

    /// <summary>
    /// ���﹥������
    /// </summary>
    public AttackData attackData;

    /// <summary>
    /// �Ƿ񱩻���
    /// </summary>
    [HideInInspector]
    public bool isCritical;

    #region ��ȡcharacterData��������
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
