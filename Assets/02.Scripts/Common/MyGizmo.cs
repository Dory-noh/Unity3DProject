using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public enum Type { NORMAL, SPAWN }
    public Type type = Type.NORMAL;
    private const string spawnPointFile = "Enemy"; //const: 값이 바뀌면 안된다. 컴파일시 값이 초기화된다. 이후에는 값을 바꿀 수 없다.
    public Color gizmoColor = Color.red;
    public float _radius = 0.1f;

    private void OnDrawGizmos() //유니티 지원 메서드
    {
        if(type == Type.NORMAL)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f,spawnPointFile,true); //true: 아이콘이 3D 공간에서 항상 카메라를 향하도록 합니다.
                            //위치(현재 오브젝트 좌표에서 y축으로 1 위로 이동한 곳, 파일-이미지 이름, 스케일 적용 여부)
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
