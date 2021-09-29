using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// ��������ģ��
    /// </summary>
    public CharacterData templateData;

    /// <summary>
    /// �����������
    /// </summary>
    private CharacterData characterData;

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

    #region ����ս����ֵ
    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="attacker">������</param>
    public void TakeDamage(CharacterStats attacker)
    {
        // ����˺�Ϊ0
        int damage = Mathf.Max(attacker.CurrentDamage() - this.CurrentDefense,0);

        // ��֤��СѪ�����ᵽ����
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        // ������ʱ���ֱ�������
        if (attacker.isCritical)
        {
            this.GetComponent<Animator>().SetTrigger("GetHit");
        }

        // TODO: ����UI
        // TODO: ����ֵ����
    }

    /// <summary>
    /// ����ʵʱ�˺�ֵ
    /// </summary>
    /// <returns></returns>
    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);

        // ���㱩���˺�
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int) coreDamage;
    }

    #endregion

    void Awake()
    {
        // ��ʼ������ģ��
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }
}
