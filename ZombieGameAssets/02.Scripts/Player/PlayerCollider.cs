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
    //isTrigger 체크 했을 때 충돌 감지 해주는 유니티 지원 메서드
    private void OnTriggerEnter(Collider other) //콜백 함수 
    {       //충돌한 게임오브젝트 태그가 Player와 같느냐
        if(other.gameObject.tag =="Player")
        {
            _light.enabled = true;
            // 라이트 켠다
            source.PlayOneShot(onOffClip, 1.0f);
        }
        
    }
    private void OnTriggerExit(Collider other) //콜백 함수 
    {       
        //충돌한 게임오브젝트 태그가 Player와 같느냐
        if (other.gameObject.tag == "Player")
        {
            _light.enabled = false;
            // 라이트 켠다
            source.PlayOneShot(onOffClip, 1.0f);
        }

    }
   
}
