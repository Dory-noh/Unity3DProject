using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//1.�Ÿ� : �÷��̾� ��ġ �� ���̷��� ��ġ
//2.�÷��̾� ���� : NavMeshAgent ���۳�Ʈ
//3. ���� �Ÿ� ���� �Ÿ� 
//4. �ִϸ����͸� �̿��ؼ� �ִϸ��̼� ����
public class SkeletonCtrl : MonoBehaviour
{
    public Transform playerTr;
    public Transform skeletonTr;
    public NavMeshAgent navi;
    public Animator animator;
    public float attackDist = 3.0f;
    public float traceDist = 20f;
    public SkeletonDamage skeletonDamage;
    void Start()
    {             //���̶�Ű���� Player�±׸� ���� ������Ʈ�� ��ġ�� ã��
        playerTr = GameObject.FindWithTag("Player").transform;
        skeletonTr = GetComponent<Transform>();
        // ���� �ڽ��� ������Ʈ �ȿ� �ִ� �ش� ���۳�Ʈ�� �ִ� ��. �ʱ�ȭ �Ѵ�.
        navi = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skeletonDamage = GetComponent<SkeletonDamage>();
    }
    void Update()
    {
        if (skeletonDamage.isDie == true) return;

        float dist = Vector3.Distance(playerTr.position, skeletonTr.position);
        if (dist <= attackDist) //���� ���� ���̸� 
        {
            navi.isStopped = true;// �׺� ���� ���� 
            animator.SetBool("IsAttack", true);
            Quaternion rot = Quaternion.LookRotation(playerTr.position - skeletonTr.position);
            skeletonTr.rotation = Quaternion.Slerp(skeletonTr.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (dist <= traceDist) //���� ���� ���̸�
        {
            navi.isStopped = false; //���� Ȱ��ȭ 
            navi.destination = playerTr.position; //���� ����� �÷��̾ �ȴ�.
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsTrace",true);
        }
        else
        {
            navi.isStopped = true;
            animator.SetBool("IsTrace",false);
        }
    }
}
