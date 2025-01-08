using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; //canvas 렌더링 카메라
    private Canvas canvas; //캔버스
    private RectTransform rectParent; //부모Rect
    private RectTransform rectHp; //자식Rect

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;
    //public 이라고 선언하였지만 inspector창에선 보이지 않는다.
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>(); //이건 왜 하는 거지?
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    void LateUpdate() //카메라가 캔버스의 이미지를 따라가야 하므로 LateUpdate사용함
    {

        // World 좌표 -> 스크린 좌표 -> 로컬 좌표
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); //3D를 2D로 변환
        if(screenPos.z < 0.0f) //타겟이 카메라를 등지고 있는 경우
        {
            screenPos.z *= -1.0f; //등지고 있는 카메라는 바라보지 않도록 함.
            
        }
        var localPos = Vector2.zero; //2D좌표이므로 Vector2를 사용한다.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
    }
}
