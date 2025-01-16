using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Light _light;
    public AudioSource source;
    public AudioClip onOffClip;
    void Start()
    {
        
    }
    //isTrigger üũ ���� �� �浹 ���� ���ִ� ����Ƽ ���� �޼���
    private void OnTriggerEnter(Collider other) //�ݹ� �Լ� 
    {       //�浹�� ���ӿ�����Ʈ �±װ� Player�� ������
        if(other.gameObject.tag =="Player")
        {
            _light.enabled = true;
            // ����Ʈ �Ҵ�
            source.PlayOneShot(onOffClip, 1.0f);
        }
        
    }
    private void OnTriggerExit(Collider other) //�ݹ� �Լ� 
    {       
        //�浹�� ���ӿ�����Ʈ �±װ� Player�� ������
        if (other.gameObject.tag == "Player")
        {
            _light.enabled = false;
            // ����Ʈ �Ҵ�
            source.PlayOneShot(onOffClip, 1.0f);
        }

    }
   
}
