using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform mainCam;
    public Transform canvasTr;
    void Start()
    {
        canvasTr = GetComponent<Transform>();
        mainCam = Camera.main.transform;
    }
    void Update()
    {
        canvasTr.LookAt(mainCam);
    }
}
