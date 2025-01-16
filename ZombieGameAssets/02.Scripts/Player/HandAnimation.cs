using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public Animation HandAni;
    public Light FlashLight;
    public AudioSource source; // 오디오소스 스피커
    public AudioClip FlashSound; //사운드 클립
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
            //static을 붙여 선언한 변수 -> 클래스명.변수명 으로 불러온다.
            //Debug.Log("재장전");
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload() //재장전 중에는 총을 쏘면 안된다.
    {
        isReload = true;
        HandAni.Stop("fire");
        HandAni.CrossFade("pump1", 0.3f); //#blend_animation 지금 하려는 애니메이션 동작과 직전 동작 사이에 0.3초라는 시간 동안 두 애니메이션을 혼합하여 애니메이션을 부드럽게 동작하는 기법

        yield return new WaitForSeconds(0.5f);
        
        FireBullet.bulletCount = 0;
        GetComponent<FireBullet>().ShowBulletCount();
        isReload = false;
    }
    private void FlashOnOff()
    {
        FlashLight.enabled = !FlashLight.enabled;
        source.PlayOneShot(FlashSound, 1.0f);
        //오디오클립 , 볼륨
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
