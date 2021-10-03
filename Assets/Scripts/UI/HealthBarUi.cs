using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    /// <summary>
    /// 血量条组件
    /// </summary>
    [Tooltip("血量条组件")]
    public GameObject HealthUiPrefab;

    /// <summary>
    /// 血量条位置
    /// </summary>
    [Tooltip("血量条位置")]
    public Transform BarPoint;

    /// <summary>
    /// 血量滑动条
    /// </summary>
    Image healthSlider;

    /// <summary>
    /// 当前生成的血量条位置
    /// </summary>
    Transform uiBar;

    /// <summary>
    /// 摄像机
    /// </summary>
    Transform c;

    /// <summary>
    /// 是否永久可见
    /// </summary>
    public bool AlwaysVisible;

    public float VisibleTime;

    CharacterStats characterStats;

    private float timeLeft;
    private void Awake()
    {
        characterStats= GetComponent<CharacterStats>();
        characterStats.UpdateHealthBarOnTakeDamage += CharacterStats_UpdateHealthBarOnTakeDamage;
    }

    private void OnEnable()
    {
        c = Camera.main.transform;
        foreach(var canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                uiBar = Instantiate(HealthUiPrefab, canvas.transform).transform;
                healthSlider = uiBar.GetChild(0).GetComponent<Image>();
                // 是否永久可见
                uiBar.gameObject.SetActive(AlwaysVisible);
                break;
            }
        }
    }

    /// <summary>
    /// 订阅当前对象受攻击时的事件
    /// </summary>
    /// <param name="currentHealth">当前血量</param>
    /// <param name="maxHealth">总血量</param>
    private void CharacterStats_UpdateHealthBarOnTakeDamage(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(uiBar.gameObject);
        }
        float s =(float)currentHealth / maxHealth;
        healthSlider.fillAmount = s;
        uiBar.gameObject.SetActive(true);
        timeLeft = VisibleTime;
    }

    private void LateUpdate()
    {
        if (uiBar != null)
        {
            uiBar.position = BarPoint.position;
            // 血条永远面对摄像机
            uiBar.forward = -c.forward;
            if(timeLeft<=0 && !AlwaysVisible)
            {
                uiBar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }
}
