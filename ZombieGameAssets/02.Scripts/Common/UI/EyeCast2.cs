using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//광선이 적에게 닿았는지 체크한다.
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
        //광선에 감지가 되면 CrossHair2 클래스에 있는 isGaze변수를 true로 만든다. 감지 안되면 false
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
