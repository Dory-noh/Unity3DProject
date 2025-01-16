using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray;//광선 자료형 // 구조체
    private RaycastHit hit;
    public CrossHair2 crosshair;
    //어떤 오브젝트가 광선에 충돌 감지와 위치 값과 거리 등을 반환하는 자료형(구조체)이다.

    void Start()
    {
        crosshair = GameObject.Find("Image-CrossHair").GetComponent<CrossHair2>(); //hierarchy에서 오브젝트명을 이용해 오브젝트를 찾고 그 오브젝트가 가진 클래스를 연결한다.
        tr = transform; //줄여 쓰기 위해 이렇게 씀
    }


    void Update()
    {
                         //위치       //방향
        ray = new Ray(tr.position, tr.forward);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue);
        //충돌 감지 했나요?
        if(Physics.Raycast(ray, out hit, 20f, 1<<6 | 1<<7 | 1<<8)) //out: 출력 전용 매개변수 //메서드 오버로딩: 위에서 ray의 위치와 방향을 잡아줬기 때문에 또 작성할 필요 없다. //bit연산자 : 무조건 1로 시작, 연산자 하나만 넣는다.(| &)
        {
            crosshair.isGaze = true;
        }
        //충돌 감지 못했다면..
        else
        {
            crosshair.isGaze = false;
        }
    }
}
