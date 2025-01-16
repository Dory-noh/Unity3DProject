using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
//1. �÷��̾�� ������ �Ÿ� : �÷��̾������� ���������� 
//2. �ʿ��� ���۳�Ʈ : �ݶ��̴� ,������ٵ� 
//3. ���� :NavMesh agent 
public class ZombieCtrl : MonoBehaviour
{
    public Transform playerTr; //�÷��̾� ��ġ
    public Transform zombieTr; //���� ��ġ 
    public NavMeshAgent navie; //���� �ϴ� ����
    public float attackdist = 2.8f; //���� �Ÿ� 
    public float tracedist = 20f; //���� ������ �Ÿ� 
    public Animator animator;
    public ZombieDamage damage;
    void Start()
    {
        damage = GetComponent<ZombieDamage>();
        playerTr = GameObject.FindWithTag("Player").transform;
        zombieTr = transform;
        //���̶�Ű ���� Player �±׸� ���� Ʈ�������� ������ �´�.
    }
    void Update()
    {
        if (damage.isDie == true) return;

        // �Ÿ��� ���� ���� �÷��̾��� �Ÿ� 
        float dist =Vector3.Distance(playerTr.position, zombieTr.position); 
        if (dist <= attackdist) //�Ÿ��� ���ݹ��� ���̶��....
        {
            navie.isStopped = true; //���� ���� 
            animator.SetBool("IsAttack",true);              //Ÿ�� ������ - �ڱ� �ڽ� ��ġ = ���� -> Ÿ�� �������� ȸ���Ѵ�.
            Quaternion rot = Quaternion.LookRotation(playerTr.position - zombieTr.position);
            zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * 3);
                                //��麸�� ��������
        }
        else if(dist <= tracedist)
        {
            navie.isStopped = false; //���� ���� 
            navie.destination = playerTr.position;
            // ���� �� ��  = �÷��̾� ������
            animator.SetBool("IsAttack",false); //���� �ִ� x
            animator.SetBool("IsTrace",true ); // �ȱ� �ִ� o
        }
        else
        {
            animator.SetBool("IsTrace", false);
            navie.isStopped = true;
        }

    }
}
