using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixToSceen : MonoBehaviour
{
    public RectTransform uiElement;  // UI Ԫ��
    Vector3 targetPos;
    private void Start()
    {
        targetPos = Vector3.one;
        targetPos.z = 10f;
    }

    private void OnEnable()
    {
        InputManage.Instance.MoveCameraStateEvent.AddListener(UpdatePosition);
    }

    [SerializeField]
    bool openUpdate;
    void UpdatePosition(bool open)
    {
        openUpdate = open;
    }
    void LateUpdate()
    {
        if (openUpdate) return;
        // ��ȡ UI Ԫ�ص���Ļλ��
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, uiElement.position);

        // ����Ļλ��ת��Ϊ��������
        targetPos.x = screenPos.x;
        targetPos.y = screenPos.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(targetPos);

        // ����ģ�͵�λ��
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
