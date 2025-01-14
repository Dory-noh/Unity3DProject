using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] Transform tr;
    void Start()
    {
        tr = transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.enabled = false;
    }

    //���� �޼���� ����ڽ��ϴ�
    public void FireRay()
    {
                                        //���� ��ġ���� ���� �÷ȴ�.
        Ray ray = new Ray(tr.position+(Vector3.up*0.02f), tr.forward);
        RaycastHit hit;
        //���� ��ǥ(�������� ���� ��ǥ�� ����)�� ���� ��ǥ�� �ű��.
        lineRenderer.SetPosition(0, tr.InverseTransformPoint(ray.origin));
        //terrain�� ���� ��� lineRenderer ǥ����
        if (Physics.Raycast(ray, out hit, 200f, 1<<6))
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(hit.point));
        }
        //�� ���� ���
        else 
        {
            //200��ŭ lineRenderer ǥ���ϰڴ�.
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(200f)));
        }
        StartCoroutine(ShowLaserBeam());
    }
    IEnumerator ShowLaserBeam()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        lineRenderer.enabled = false;
    }
}

