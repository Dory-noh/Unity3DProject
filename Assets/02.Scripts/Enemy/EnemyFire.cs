using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] AudioClip fireClip;
    [SerializeField] AudioClip reloadClip;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animator;
    [SerializeField] GameObject fireEffect;
    [SerializeField] Transform tr;
    [SerializeField] Transform playerTr;

    private readonly string playerTag = "Player";
    public E_GunData gunData_Ak;

    private float nextFire = 0f; //������ �߻��� �ð� ���� ����
    private readonly float damping = 10; //�÷��̾ ���� ȸ���� �ӵ�
    public bool isFire = false;

    void Start()
    {
        fireClip = gunData_Ak.shotClip;
        reloadClip = gunData_Ak.reloadClip;
        fireEffect = Resources.Load<GameObject>("Effect/FlareMobile");
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
    }


    void Update()
    {
        if (isFire)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + gunData_Ak.fireRate + Random.Range(0.0f, 0.3f);
            } 
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position); //Ÿ��-�ڱ� �ڽ� ��ġ => ����
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping*Time.deltaTime);
        }
    }
    void Fire()
    {
        source.PlayOneShot(fireClip, 1.0f);
    }
}
