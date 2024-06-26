using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixToSceen : MonoBehaviour
{
    public RectTransform uiElement;  // UI 元素
    Vector3 targetPos;
    private void Start()
    {
        targetPos = Vector3.one;
    }

    void LateUpdate()
    {
        // 获取 UI 元素的屏幕位置
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, uiElement.position);

        // 将屏幕位置转换为世界坐标
        targetPos.x = screenPos.x;
        targetPos.y = screenPos.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(targetPos);

        // 设置模型的位置
        transform.position = worldPos;
    }













  
    void LateUpdate1()
    {
        ViewWorldPoint();
    }
     
    void ViewWorldPoint()
    {
        transform.position = Camera.main.ViewportToWorldPoint(targetPos);
    }

    void OnDrawGizmosSelected1()
    {
        Vector3 p = Camera.main.ViewportToWorldPoint(targetPos);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);
    }

}
