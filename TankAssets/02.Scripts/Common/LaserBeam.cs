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

    //공용 메서드로 만들겠슴니다
    public void FireRay()
    {
                                        //현재 위치에서 조금 올렸다.
        Ray ray = new Ray(tr.position+(Vector3.up*0.02f), tr.forward);
        RaycastHit hit;
        //월드 좌표(레이저는 월드 좌표에 맞음)를 로컬 좌표로 옮긴다.
        lineRenderer.SetPosition(0, tr.InverseTransformPoint(ray.origin));
        //terrain에 맞은 경우 lineRenderer 표시함
        if (Physics.Raycast(ray, out hit, 200f, 1<<6))
        {
            lineRenderer.SetPosition(1, tr.InverseTransformPoint(hit.point));
        }
        //안 맞은 경우
        else 
        {
            //200만큼 lineRenderer 표시하겠다.
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

