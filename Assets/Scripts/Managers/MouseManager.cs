using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    /// <summary>
    /// ���ͼƬ
    /// </summary>
    public Texture2D point, doorway, attack, target, arrow;

    /// <summary>
    /// �����ʱ���¼�
    /// </summary>
    public event Action<Vector3> OnMouseClicked;

    /// <summary>
    /// �����������ʱ
    /// </summary>
    public event Action<GameObject> OnEnemyClicked;

    RaycastHit hit;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        // ������ͷ�������ߵ����λ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // �л������ͼ
            switch (hit.collider.gameObject.tag)
            {
                
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }


    }


    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            Debug.Log("��������");
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hit.point);
            }

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hit.collider.gameObject);
            }
        }
    }
}
