using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public Color _color = Color.blue;
    public float _radius = 0.2f;
    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
