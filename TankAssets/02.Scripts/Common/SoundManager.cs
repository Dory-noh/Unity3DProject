using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public bool isMute = false; //음소거
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void playSFX(Vector3 pos, AudioClip bgm, bool isLoop) //배경 음악이나 끊임없이 울리는 소리를 담당한다.
    {
        if (isMute) return; //음소거라면 소리를 출력하지 않는다.
        //오브젝트 자체를 동적 할당한다.
        GameObject soundObj = new GameObject("BackGround Music or Lotor Sound");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        //Get이 아니라 AddComponent -> 객체가 해당 컴포넌트를 가지고 있지 않으면 하나 생성한다.

        audioSource.clip = bgm;
        audioSource.loop = isLoop;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 100f;
        audioSource.volume = 1.0f;
        audioSource.Play();
        if (!isLoop) Destroy(soundObj, 0.5f);
    }

    //오버로딩 : 메서드 이름이 같아도 매개변수의 자료형, 개수가 다르면 다르게 작동하도록 만들 수 있다.
    //public void playSFX(Vector3 pos, AudioClip bgm) //배경 음악이나 끊임없이 울리는 소리를 담당한다.
    //{
    //    if (isMute) return; //음소거라면 소리를 출력하지 않는다.
    //    //오브젝트 자체를 동적 할당한다.
    //    GameObject soundObj = new GameObject("BackGround Music or Lotor Sound");
    //    soundObj.transform.position = pos;
    //    AudioSource audioSource = soundObj.AddComponent<AudioSource>();
    //    //Get이 아니라 AddComponent -> 객체가 해당 컴포넌트를 가지고 있지 않으면 하나 생성한다.

    //    audioSource.clip = bgm;
    //    audioSource.minDistance = 20f;
    //    audioSource.maxDistance = 100f;
    //    audioSource.volume = 1.0f;
    //    audioSource.Play();
    //    Destroy(soundObj, 0.3f);
    //}
}
