using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    /// <summary>
    /// Ѫ�������
    /// </summary>
    [Tooltip("Ѫ�������")]
    public GameObject HealthUiPrefab;

    /// <summary>
    /// Ѫ����λ��
    /// </summary>
    [Tooltip("Ѫ����λ��")]
    public Transform BarPoint;

    /// <summary>
    /// Ѫ��������
    /// </summary>
    Image healthSlider;

    /// <summary>
    /// ��ǰ���ɵ�Ѫ����λ��
    /// </summary>
    Transform uiBar;

    /// <summary>
    /// �����
    /// </summary>
    Transform c;

    /// <summary>
    /// �Ƿ����ÿɼ�
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
                // �Ƿ����ÿɼ�
                uiBar.gameObject.SetActive(AlwaysVisible);
                break;
            }
        }
    }

    /// <summary>
    /// ���ĵ�ǰ�����ܹ���ʱ���¼�
    /// </summary>
    /// <param name="currentHealth">��ǰѪ��</param>
    /// <param name="maxHealth">��Ѫ��</param>
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
            // Ѫ����Զ��������
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
