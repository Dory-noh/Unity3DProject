using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public enum Type { NORMAL, SPAWN }
    public Type type = Type.NORMAL;
    private const string spawnPointFile = "Enemy"; //const: ���� �ٲ�� �ȵȴ�. �����Ͻ� ���� �ʱ�ȭ�ȴ�. ���Ŀ��� ���� �ٲ� �� ����.
    public Color gizmoColor = Color.red;
    public float _radius = 0.1f;

    private void OnDrawGizmos() //����Ƽ ���� �޼���
    {
        if(type == Type.NORMAL)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f,spawnPointFile,true); //true: �������� 3D �������� �׻� ī�޶� ���ϵ��� �մϴ�.
                            //��ġ(���� ������Ʈ ��ǥ���� y������ 1 ���� �̵��� ��, ����-�̹��� �̸�, ������ ���� ����)
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
