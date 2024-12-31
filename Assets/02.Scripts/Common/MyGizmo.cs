using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.red;
    public float _radius = 0.1f;

    private void OnDrawGizmos() //유니티 지원 메서드
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
