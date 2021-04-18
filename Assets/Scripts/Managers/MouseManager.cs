using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    /// <summary>
    /// 鼠标图片
    /// </summary>
    public Texture2D point, doorway, attack, target, arrow;

    /// <summary>
    /// 鼠标点击时的事件
    /// </summary>
    public event Action<Vector3> OnMouseClicked;

    /// <summary>
    /// 鼠标点击到敌人时
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
        // 主摄像头发射射线到鼠标位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // 切换鼠标贴图
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
            Debug.Log("点击了鼠标");
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
