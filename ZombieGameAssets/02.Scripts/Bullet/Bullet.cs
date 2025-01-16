using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 3000f;
    public Rigidbody rb;
    private TrailRenderer trail;
    private void Awake()
    {
   
    }
    private void OnEnable() //해당 오브젝트가 활성화 되면 발사한다.
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        rb.AddForce(transform.forward * Speed);
        Invoke("BulletDisable", 3f);
    }
    
    void Start()
    {

        
        
        //Destroy(this.gameObject, 3.0f);
        //발사되고 3초후에 사라짐 
    }

    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable() //해당 오브젝트가 비활성화되었을 때 생성
    {
        trail.Clear();
        rb.Sleep(); //rigidbody 작동 멈춤
    }


}
