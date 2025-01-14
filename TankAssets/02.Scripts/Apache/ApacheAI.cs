using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ApacheAI : MonoBehaviour
{
    public enum ApacheState { PATROL, ATTACK, DESTROY}
    public ApacheState state = ApacheState.PATROL;
    [SerializeField] private AudioClip RotorSound;
    [Header("Patrol")]
    [SerializeField] private List<Transform> patrolList;
    [Header("Speed")]
    [SerializeField] float rotSpeed = 15f, moveSpeed = 10f; //ȸ�� �ӵ�
    Transform tr;

    int currentIdx = 0;
    public bool isSearch = true;
    void Start()
    {
        isSearch = true;
        tr = GetComponent<Transform>();
        var patrolObj = GameObject.Find("PatrolPoint");
        if (patrolObj != null)
        {
            patrolObj.transform.GetComponentsInChildren<Transform>(patrolList);
            patrolList.RemoveAt(0);
        }
        RotorSound = Resources.Load("Sounds/RotorSound") as AudioClip;
        SoundManager.instance.playSFX(tr.position, RotorSound, true);
    }

    void LateUpdate()
    {
        if (isSearch)
        {
            PatrolMove();
            CheckIndex();
        }
        else //Tank(Player) ã���� ����
        {
            Attack();
        }
        StartCoroutine(Search());
    }
    private void PatrolMove()
    {
        state = ApacheState.PATROL;
        Vector3 direction = (patrolList[currentIdx].position - tr.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, Time.deltaTime * rotSpeed);
        tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void CheckIndex()
    {
        if (Vector3.Distance(tr.position, patrolList[currentIdx].position) < 3f)
        {
            currentIdx = (currentIdx + 1) % patrolList.Count;
        }
        
    }
    IEnumerator Search()
    {
        if (GameObject.FindWithTag("TANK") == null) yield return new WaitForSeconds(3f);
        float TankFindDist = (GameObject.FindWithTag("TANK").transform.position - tr.position).magnitude; //�Ÿ��� ���� -> Vector3���� float�� ��ȯ
        if(TankFindDist <= 150f)
        {
            isSearch = false;
        }
        else
        {
            isSearch = true;
        }
    }
    
    void Attack() //Tank ���� ��
    {
        state = ApacheState.ATTACK;
        //Debug.Log("������");
        Vector3 targetDist = (GameObject.FindWithTag("TANK").transform.position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(targetDist.normalized), Time.deltaTime * rotSpeed);
        //80���� �̳��� ��ũ�� �ִٸ�, ������ �� �Ѿƴٴ��� ����

        if(targetDist.magnitude > 150f)
        {
            isSearch = true;
        }
    }
}
