using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] TrailRenderer trailRenderer;
    public float Speed = 2000f;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        rb.AddForce(transform.forward * Speed);
        //Destroy(gameObject, 3.0f);
        Invoke("BulletDisable", 3.0f);
    }
    void Start()
    {
        
    }
    void BulletDisable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
        rb.Sleep(); //리지드바디의 작동을 잠시 멈춘다.
    }
}
