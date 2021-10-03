using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : Singleton<MouseManager>
{
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
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }


    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hit.point);
            }

            if (new[] { "Enemy", "Attackable" }.Contains(hit.collider.gameObject.tag))
            {
                OnEnemyClicked?.Invoke(hit.collider.gameObject);
            }
        }
    }
}
