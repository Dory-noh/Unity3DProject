using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float Height = 15; //카메라 높이
    [SerializeField] float Distance = 20; //카메라 거리
    [SerializeField] float moveDamping = 10f; //카메라 움직임 충격 완화하여덜 떨리게/부드럽게 한다.
    [SerializeField] float rotateDamping = 15f;
    [SerializeField] Transform tr;
    public float targetOffset = 2.0f;

     void Start()
    {
        tr = GetComponent<Transform>();
    }

    void LateUpdate()//플레이어가 이동한 후에 카메라가 따라 가야 하니깐 LateUpdate 사용(LateUpdate는 Update, FixedUpdate 실행 다음에 실행된다.)
    {
        var camPos = target.position - (target.forward * Distance) //타겟 뒤에 Distance만큼 떨어져서
                                     + (target.up * Height); //타겟 위에 Height만큼 올라가서
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping); //A에서 B로 움직일 때 moveDamping(*Time.deltaTime)만큼 부드럽게 움직인다.
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);//A에서 B로 rotateDamping(*Time.deltaTime)만큼 부드럽게 회전한다.
        tr.LookAt(target.position+(target.up*targetOffset)); //위치값 더하거나 뺌, 거리는 곱한다.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position+(target.up*targetOffset),0.1f);
        Gizmos.DrawLine(target.position + (target.up * targetOffset), tr.position);
    }
}
