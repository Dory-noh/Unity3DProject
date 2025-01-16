using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpark : MonoBehaviour
{
    public GameObject spark; //����Ʈ ���ӿ�����Ʈ 
    public AudioSource source;
    public AudioClip  sparkClip;
     
    // isTrigger üũ �ȵǾ��� �� �ڵ� ȣ��  block
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
                // ������ ȸ�� ���� ���� 
            Destroy(spk,2.5f);
        }
    }

}
