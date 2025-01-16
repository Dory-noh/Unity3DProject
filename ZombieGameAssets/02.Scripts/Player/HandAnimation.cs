using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public Animation HandAni;
    public Light FlashLight;
    public AudioSource source; // ������ҽ� ����Ŀ
    public AudioClip FlashSound; //���� Ŭ��
    public bool isRunning = false;
    public bool isReload = false;
    void Start()
    {
        
    }
    void Update()
    {
        RunAni();

        if(Input.GetKeyDown(KeyCode.F))
        {
            FlashOnOff();
        }
        if (FireBullet.bulletCount == 10)
        {
            //static�� �ٿ� ������ ���� -> Ŭ������.������ ���� �ҷ��´�.
            //Debug.Log("������");
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload() //������ �߿��� ���� ��� �ȵȴ�.
    {
        isReload = true;
        HandAni.Stop("fire");
        HandAni.CrossFade("pump1", 0.3f); //#blend_animation ���� �Ϸ��� �ִϸ��̼� ���۰� ���� ���� ���̿� 0.3�ʶ�� �ð� ���� �� �ִϸ��̼��� ȥ���Ͽ� �ִϸ��̼��� �ε巴�� �����ϴ� ���

        yield return new WaitForSeconds(0.5f);
        
        FireBullet.bulletCount = 0;
        GetComponent<FireBullet>().ShowBulletCount();
        isReload = false;
    }
    private void FlashOnOff()
    {
        FlashLight.enabled = !FlashLight.enabled;
        source.PlayOneShot(FlashSound, 1.0f);
        //�����Ŭ�� , ����
    }

    private void RunAni()
    {
        if (Input.GetKey(KeyCode.LeftShift) &&
                    Input.GetKey(KeyCode.W))
        {
            isRunning = true;
            HandAni.Play("running");
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            HandAni.Play("runStop");
        }
    }
}
