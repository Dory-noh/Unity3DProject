using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    Transform tr;//상황에 맞게 조준점 이미지 크기가 변하도록 할 것이므로
    public Image crosshair; //CrossHair라 적으면 클래스명과 같아서 오류난다.
    private float startTime; //조준점이 커지기 시작하는 시간 저장 현재시 - 과거시 = 흘러간 시간
    public float duration = 0.2f; //조준점 커지는 속도
    public float minSize = 0.8f; //최소 크기
    public float maxSize = 1.2f; //최대 크기
    //초기 색상
    private Color originColor = new Color(1f, 1f, 1f, 0.8f);
    public Color gazeColor = Color.red; //gaze: 응시하다, 바라보다., Enemy를 겨눌 때 색상
    public bool isGaze = false; //광선을 감지했는가 T/F
    // Start is called before the first frame update
    void Start()
    {
      crosshair = GetComponent<Image>();
        tr = transform;
        startTime = Time.time;
        tr.localScale = Vector3.one * minSize;
        //조준점 크기는 최소 사이즈이다. Vector3.one =  x,y,z의 크기가 동일한 비율이 되도록 한다. 2D게임에도 많이 쓰인다.
        crosshair.color = originColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGaze) //광선이 적에게 닿았다면
        {
            float t = (Time.time - startTime) / duration; //현재시 - 과거시 = 흘러간 시간       흘러간 시간/0.2f
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t); //최소 크기에서 최대 크기로 t만큼 점진적으로 커진다.
            crosshair.color = gazeColor;
        }
        else //광선이 적에게 닿지 않았다면
        {
            tr.localScale = Vector3.one * minSize;
            crosshair.color = originColor;
            startTime = Time.time;
        }
    }
}
