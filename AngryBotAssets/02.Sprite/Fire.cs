using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    private PhotonView pv = null;
    private PlayerInput input;

    void Start()
    {
        firePos = transform.GetChild(2).GetChild(0).transform;
        bulletPrefab = Resources.Load<GameObject>("Bullet");
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        pv = GetComponent<PhotonView>();
        input=GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(pv.IsMine && input.isMouseClick)
        {
            FireBullet();//�ڱ� �ڽ� �ش� �޼��� ȣ��
            pv.RPC("FireBullet", RpcTarget.Others, null); //�ڽ� ���� ������ Ŭ���̾�Ʈ���� �ش� �޼��� ȣ��
        }
    }

    [PunRPC]
    void FireBullet()
    {
        //muzzleFlash �÷��� ���� �ƴ϶�� ����Ѵ�.
        if(!muzzleFlash.isPlaying) muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab,firePos.position, firePos.rotation);
    }
}
