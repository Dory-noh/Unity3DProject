using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    public SkinnedMeshRenderer Spas12;
    public MeshRenderer[] Ak_47;//Ak_47안에 MeshRenderer Component가 3개 있다.
    public MeshRenderer[] M4A1;
    public Animation ComBatSgAni;
    public bool isHaveM4A1 = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            KeyOne();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            KeyTwo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            KeyThree();
        }
    }

    private void KeyThree()
    {
        ComBatSgAni.Play("draw");
        for (int i = 0; i < Ak_47.Length; i++)
        {
            Ak_47[i].enabled = false;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
        Spas12.enabled = true;
        isHaveM4A1 = false;
    }

    private void KeyTwo()
    {
        ComBatSgAni.Play("draw");
        for (int i = 0; i < Ak_47.Length; i++)
        {
            Ak_47[i].enabled = false;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = true;
        }
        Spas12.enabled = false;
        isHaveM4A1 = true;
    }

    private void KeyOne()
    {
        ComBatSgAni.Play("draw");
        for (int i = 0; i < Ak_47.Length; i++)
        {
            Ak_47[i].enabled = true;
        }
        for (int i = 0; i < M4A1.Length; i++)
        {
            M4A1[i].enabled = false;
        }
        Spas12.enabled = false;
        isHaveM4A1 = false;
    }
}
