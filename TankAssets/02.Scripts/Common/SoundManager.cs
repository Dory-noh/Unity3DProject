using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public bool isMute = false; //���Ұ�
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

    public void playSFX(Vector3 pos, AudioClip bgm, bool isLoop) //��� �����̳� ���Ӿ��� �︮�� �Ҹ��� ����Ѵ�.
    {
        if (isMute) return; //���ҰŶ�� �Ҹ��� ������� �ʴ´�.
        //������Ʈ ��ü�� ���� �Ҵ��Ѵ�.
        GameObject soundObj = new GameObject("BackGround Music or Lotor Sound");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        //Get�� �ƴ϶� AddComponent -> ��ü�� �ش� ������Ʈ�� ������ ���� ������ �ϳ� �����Ѵ�.

        audioSource.clip = bgm;
        audioSource.loop = isLoop;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 100f;
        audioSource.volume = 1.0f;
        audioSource.Play();
        if (!isLoop) Destroy(soundObj, 0.5f);
    }

    //�����ε� : �޼��� �̸��� ���Ƶ� �Ű������� �ڷ���, ������ �ٸ��� �ٸ��� �۵��ϵ��� ���� �� �ִ�.
    //public void playSFX(Vector3 pos, AudioClip bgm) //��� �����̳� ���Ӿ��� �︮�� �Ҹ��� ����Ѵ�.
    //{
    //    if (isMute) return; //���ҰŶ�� �Ҹ��� ������� �ʴ´�.
    //    //������Ʈ ��ü�� ���� �Ҵ��Ѵ�.
    //    GameObject soundObj = new GameObject("BackGround Music or Lotor Sound");
    //    soundObj.transform.position = pos;
    //    AudioSource audioSource = soundObj.AddComponent<AudioSource>();
    //    //Get�� �ƴ϶� AddComponent -> ��ü�� �ش� ������Ʈ�� ������ ���� ������ �ϳ� �����Ѵ�.

    //    audioSource.clip = bgm;
    //    audioSource.minDistance = 20f;
    //    audioSource.maxDistance = 100f;
    //    audioSource.volume = 1.0f;
    //    audioSource.Play();
    //    Destroy(soundObj, 0.3f);
    //}
}
