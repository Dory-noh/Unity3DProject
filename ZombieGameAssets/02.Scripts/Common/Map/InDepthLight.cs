using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDepthLight : MonoBehaviour
{
    public Light _light;
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            _light.enabled = true;
            source.PlayOneShot(clip, 1.0f);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _light.enabled = false;
            source.PlayOneShot(clip, 1.0f);
        }
    }

}
