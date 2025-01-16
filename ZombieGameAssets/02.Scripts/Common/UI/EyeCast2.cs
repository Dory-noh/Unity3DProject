using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������ ��Ҵ��� üũ�Ѵ�.
public class EyeCast2 : MonoBehaviour
{
    private Transform tr;
    public CrossHair2 crosshair;
    private RaycastHit hit;
    private Ray ray;
    void Start()
    {
        tr = transform;
        crosshair = GameObject.Find("Image-CrossHair").GetComponent<CrossHair2>();
    }

    void Update()
    {
        ray = new Ray(tr.position, tr.forward);
        Debug.DrawRay(ray.origin, ray.direction);
        //������ ������ �Ǹ� CrossHair2 Ŭ������ �ִ� isGaze������ true�� �����. ���� �ȵǸ� false
        if(Physics.Raycast(ray, out hit,20f,1<<6|1<<7|1<<8))
        {
            crosshair.isGaze = true;
        }
        else
        {
            crosshair.isGaze = false;
        }
    }
}
