using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float h = 0f, v = 0f;
    public string ForwardBackward = "Vertical";
    public string LeftRight = "Horizontal";
    public bool isMouseClick = false;
    void Start()
    {
        
    }

    void Update()
    {
        h = Input.GetAxis(LeftRight);
        v = Input.GetAxis(ForwardBackward);
        isMouseClick = Input.GetMouseButtonDown(0);
    }
}
