using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;
    private CanvasGroup canvasGroup;
    
    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;
    void Start()
    {
        canvas = GameObject.Find("UI - Canvas").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    void LateUpdate()
    {
        var ScreenPos = Camera.main.WorldToScreenPoint(targetTr.position+offset);
        if (ScreenPos.z < 0.0f)
        {
            canvasGroup.alpha = -1.0f;
            return;
        }
        canvasGroup.alpha = 1.0f;
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, ScreenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
    }
}
