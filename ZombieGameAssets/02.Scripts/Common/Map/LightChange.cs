using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour
{
    public Light whiteLight;
    public Light yellowLight;
    public Light blueLight;
  
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
        whiteLight.enabled = true;
        blueLight.enabled = false;
        yellowLight.enabled = false;
        yield return new WaitForSeconds(3f);

        whiteLight.enabled = false;
        blueLight.enabled = true;
        yellowLight.enabled = false;

        yield return new WaitForSeconds(3f);

        whiteLight.enabled = false;
        blueLight.enabled = false;
        yellowLight.enabled = true;

        yield return new WaitForSeconds(3f);
        TurnOn();
    }
}
