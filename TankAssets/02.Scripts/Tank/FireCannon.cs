using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FireCannon : MonoBehaviourPun
{
    [SerializeField] Transform firePos;
    [SerializeField] TankInput input;
    [SerializeField] LaserBeam laserBeam;
    [SerializeField] GameObject expEffect;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip explosionClip;
    bool isHit = false;
    int terrainLayer;
    public int playerid = -1; //쏜 사람을 알아야 데미지 어떻게 처리할 지 알 수 있으니깜
    Ray ray;
    void Start()
    {
        firePos = transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).transform;
        input = GetComponent<TankInput>();
        laserBeam = firePos.GetChild(0).GetComponent<LaserBeam>();
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        source = GetComponent<AudioSource>();
        shootClip = Resources.Load<AudioClip>("Sounds/enemy_PatrolMech_ShootMissile");
        explosionClip = Resources.Load<AudioClip>("Sounds/enemy_Spider_DestroyedExplosion");
        terrainLayer = LayerMask.NameToLayer("TERRAIN"); //레이어를 많이 사용하게 되는 경우 string("TERRAIN")을 int형으로 바꿔주는 것이 좋다.
    }

    void Update()
    {
        //if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        if (input.isFire)
        {
            if (photonView.IsMine) Fire(); //본인
            else photonView.RPC("Fire", RpcTarget.Others); //멀리 떨어진 네트워크 유저들은 원격 프로시저를 호출한다.
        }

        [PunRPC] //Photon Unity Network Remote Procedure Call : 원격 함수 호출
        void Fire()
        {
            playerid = photonView.OwnerActorNr; //총알이 실제로 나가는 거면 총알 아이디를 넣으면 된다.
            RaycastHit hit;
            ray = new Ray(firePos.position, firePos.forward);
            //source.PlayOneShot(shootClip, 1f);

            SoundManager.instance.playSFX(transform.position, shootClip, false);
            if (Physics.Raycast(ray, out hit, 200f))
            {
                isHit = true;
                laserBeam.FireRay(); //Terrain에 맞으면 Ray 출력
                ShowEffect(hit);
            }
            else
            {
                isHit = false;
                laserBeam.FireRay(); //맞지 않은 경우에도 이펙트가 뜨도록 한다.
                ShowEffect(hit);
            }
        }

        void ShowEffect(RaycastHit hit)
        {
            if (isHit)
            {
                Vector3 hitPoint = hit.point; //Ray가 맞은 위치
                Vector3 _normal = (firePos.position - hitPoint).normalized; //방향
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
                var effect = Instantiate(expEffect, hitPoint, rot);
                StartCoroutine(PlayExplosionClip(0.15f, hitPoint));
                Destroy(effect, 1.5f);
            }
            else
            {
                Vector3 hitPoint = ray.GetPoint(200f);
                Vector3 _normal = (firePos.position - hitPoint).normalized; //방향
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
                var effect = Instantiate(expEffect, hitPoint, rot);
                StartCoroutine(PlayExplosionClip(0.3f, hitPoint));

                Destroy(effect, 1.5f);
            }
        }
        IEnumerator PlayExplosionClip(float time, Vector3 hitPoint)
        {
            yield return new WaitForSeconds(time);
            SoundManager.instance.playSFX(hitPoint, explosionClip, false);
        }
    }
}
