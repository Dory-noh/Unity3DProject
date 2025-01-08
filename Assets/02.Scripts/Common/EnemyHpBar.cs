using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; //canvas ������ ī�޶�
    private Canvas canvas; //ĵ����
    private RectTransform rectParent; //�θ�Rect
    private RectTransform rectHp; //�ڽ�Rect

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;
    //public �̶�� �����Ͽ����� inspectorâ���� ������ �ʴ´�.
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>(); //�̰� �� �ϴ� ����?
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    void LateUpdate() //ī�޶� ĵ������ �̹����� ���󰡾� �ϹǷ� LateUpdate�����
    {

        // World ��ǥ -> ��ũ�� ��ǥ -> ���� ��ǥ
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); //3D�� 2D�� ��ȯ
        if(screenPos.z < 0.0f) //Ÿ���� ī�޶� ������ �ִ� ���
        {
            screenPos.z *= -1.0f; //������ �ִ� ī�޶�� �ٶ��� �ʵ��� ��.
            
        }
        var localPos = Vector2.zero; //2D��ǥ�̹Ƿ� Vector2�� ����Ѵ�.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
    }
}
