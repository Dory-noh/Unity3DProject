using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange2 : MonoBehaviour
{
    public Light RedLight;
    public Light BlueLight;
    public Light GreenLight;

    void Start()
    {
        TurnOn();
    }

    void TurnOn()
    {
        StartCoroutine(LightOnOff());
    }

    IEnumerator LightOnOff()
    {
        RedLight.enabled = true;
        BlueLight.enabled = false;
        GreenLight.enabled = false;
        yield return new WaitForSeconds(3f);

        RedLight.enabled = false;
        BlueLight.enabled = true;
        GreenLight.enabled = false;
        yield return new WaitForSeconds(3f);

        RedLight.enabled = false;
        BlueLight.enabled = false;
        GreenLight.enabled = true;
        yield return new WaitForSeconds(3f);

        TurnOn();
    }

}
