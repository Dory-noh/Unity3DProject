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
            FireBullet();//자기 자신 해당 메서드 호출
            pv.RPC("FireBullet", RpcTarget.Others, null); //자신 제외 나머지 클라이언트에게 해당 메서드 호출
        }
    }

    [PunRPC]
    void FireBullet()
    {
        //muzzleFlash 플레이 중이 아니라면 재생한다.
        if(!muzzleFlash.isPlaying) muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab,firePos.position, firePos.rotation);
    }
}
