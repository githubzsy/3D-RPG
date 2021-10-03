using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName = "Character Stats/Data")]
public class CharacterData : ScriptableObject
{
    #region �������
    /// <summary>
    /// ���HP
    /// </summary>
    [Header("Stats Info")]
    [Tooltip("���HP")]
    public int maxHealth;

    /// <summary>
    /// ��ǰHP
    /// </summary>
    [Tooltip("��ǰHP")]
    public int currentHealth;

    /// <summary>
    /// ��������
    /// </summary>
    [Tooltip("��������")]
    public int baseDefense;

    /// <summary>
    /// ��ǰ����
    /// </summary>
    [Tooltip("��ǰ����")]
    public int currentDefense;
    #endregion

    #region �������
    /// <summary>
    /// ���ﾭ����
    /// </summary>
    [Header("Kill")]
    [Tooltip("���ﾭ��")]
    public int killPoint;

    /// <summary>
    /// ��ǰ�ȼ�
    /// </summary>
    [Tooltip("��ǰ�ȼ�")]
    public int currentLevel;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Tooltip("��ǰ����ֵ")]
    public int currentExp;

    /// <summary>
    /// �����㾭��
    /// </summary>
    [Tooltip("�����㾭��")]
    public int baseExp;

    /// <summary>
    /// �������鱶��
    /// </summary>
    [Tooltip("��ǰ������Ҫ����ֵ����")]
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
