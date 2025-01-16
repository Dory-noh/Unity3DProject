using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider2 : MonoBehaviour
{
    public Light _light;
    public AudioSource source;
    public AudioClip onOffClip;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _light.enabled = true;
            source.PlayOneShot(onOffClip);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _light.enabled = false;
            source.PlayOneShot(onOffClip);
        }
    }

}
