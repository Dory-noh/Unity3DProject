using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    //��� �Ŀ� ���� ó���� ���� ������ �迭 ����
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

        SetPlayerVisible(false); //�װ��� 3�� �ڿ� ����� - �ٽ� �¾ �� ���̰� ��
        animator.SetBool(hashRespawn, true);

        yield return reWs; //���� ��ġ �������� ���� ��� �ð�

        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;
        transform.rotation = points[idx].rotation;

        CC.enabled = true;
        SetPlayerVisible(true);
        curHp = initHp;
    }


    //�������� �����ϰ� �����ؼ� �Ⱥ��̰� �ϰų� ���̰��Ѵ�.
    void SetPlayerVisible(bool visible)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}
