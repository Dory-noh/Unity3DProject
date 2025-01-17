using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    //사망 후에 투명 처리를 위한 렌더러 배열 선언
    private Renderer[] renderers;
    private Animator animator;
    private CharacterController CC;

    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    private WaitForSeconds ws;
    private WaitForSeconds reWs;

    private int initHp = 100;
    public int curHp = 100;
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();
        ws = new WaitForSeconds(3f);
        reWs = new WaitForSeconds(1f);
        curHp = initHp;
        SetPlayerVisible(true);
    }
    
    private void OnCollisionEnter(Collision col)
    {
        if(curHp > 0 && col.collider.CompareTag("BULLET"))
        {
            curHp -= 20;
            if(curHp <= 0)
            {
                StartCoroutine(PlayerDie());
            }
        }
    }

    IEnumerator PlayerDie()
    {
        CC.enabled = false;
        animator.SetBool(hashRespawn, false);
        animator.SetTrigger(hashDie);
        
        yield return ws;

        SetPlayerVisible(false); //죽고나서 3초 뒤에 사라짐 - 다시 태어날 때 보이게 함
        animator.SetBool(hashRespawn, true);

        yield return reWs; //생성 위치 재조정을 위한 대기 시간

        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;
        transform.rotation = points[idx].rotation;

        CC.enabled = true;
        SetPlayerVisible(true);
        curHp = initHp;
    }


    //렌더러들 투명하게 설정해서 안보이게 하거나 보이게한다.
    void SetPlayerVisible(bool visible)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}
