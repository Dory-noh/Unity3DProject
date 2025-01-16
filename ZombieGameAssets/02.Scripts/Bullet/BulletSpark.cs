using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpark : MonoBehaviour
{
    public GameObject spark; //이펙트 게임오브젝트 
    public AudioSource source;
    public AudioClip  sparkClip;
     
    // isTrigger 체크 안되었을 때 자동 호출  block
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag =="BULLET")
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            source.PlayOneShot(sparkClip, 1.0f);  
                                 //what , where
            var spk = Instantiate(spark,col.transform.position,
                Quaternion.identity);
                // 생성시 회전 없이 생성 
            Destroy(spk,2.5f);
        }
    }

}
