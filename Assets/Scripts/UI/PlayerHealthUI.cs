using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    /// <summary>
    /// 等级
    /// </summary>
    Text levelText;
    /// <summary>
    /// 生命
    /// </summary>
    Image healthSlider;
    /// <summary>
    /// 经验
    /// </summary>
    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float healthPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = healthPercent;
    }

    void UpdateExp()
    {
        float expPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = expPercent;
    }
}
