using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    private Transform tr;
    private float rotSpeed = 5f;
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        tr = transform;    
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //카메라 스크린에서 마우스 포지션 방향으로 광선을 발사
        Debug.DrawRay(ray.origin, ray.direction*100f, Color.red); //Scene에서만 보임
        if (Physics.Raycast(ray, out hit, 60f, 1<<6)) //out: 출력 전용 매개변수 //terrain만 감지-나무, 마을, 집 등은 감지 안 함
        {
            //맞은 지점의 월드 좌표를 탱크의 로컬 좌표로 바꾼다.
            Vector3 relative = tr.InverseTransformDirection(hit.point).normalized;
                                //광선 ray가 맞은 지점을 탱크에 맞는 로컬 좌표로 바꾼다.
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg; //pi*2/360 
                                                                               //역탄젠트함수(로컬 지점.x, 로컬 지점.z - y는 위쪽이니깐요 ) * pi*2/360 
            tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f); //y쪽으로만 회전한다.
        }
        //else
        //{
        //    Quaternion resetAngle = Quaternion.Euler(0, 0, 0);
        //    tr.rotation = Quaternion.Slerp(tr.rotation, resetAngle, Time.deltaTime * rotSpeed);
        //}
    }
}
