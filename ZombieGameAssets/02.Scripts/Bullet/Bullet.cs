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
    private void OnEnable() //�ش� ������Ʈ�� Ȱ��ȭ �Ǹ� �߻��Ѵ�.
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        rb.AddForce(transform.forward * Speed);
        Invoke("BulletDisable", 3f);
    }
    
    void Start()
    {

        
        
        //Destroy(this.gameObject, 3.0f);
        //�߻�ǰ� 3���Ŀ� ����� 
    }

    void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable() //�ش� ������Ʈ�� ��Ȱ��ȭ�Ǿ��� �� ����
    {
        trail.Clear();
        rb.Sleep(); //rigidbody �۵� ����
    }


}
