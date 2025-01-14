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
    public int playerid = -1; //�� ����� �˾ƾ� ������ ��� ó���� �� �� �� �����ϱ�
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
        terrainLayer = LayerMask.NameToLayer("TERRAIN"); //���̾ ���� ����ϰ� �Ǵ� ��� string("TERRAIN")�� int������ �ٲ��ִ� ���� ����.
    }

    void Update()
    {
        //if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        if (input.isFire)
        {
            if (photonView.IsMine) Fire(); //����
            else photonView.RPC("Fire", RpcTarget.Others); //�ָ� ������ ��Ʈ��ũ �������� ���� ���ν����� ȣ���Ѵ�.
        }

        [PunRPC] //Photon Unity Network Remote Procedure Call : ���� �Լ� ȣ��
        void Fire()
        {
            playerid = photonView.OwnerActorNr; //�Ѿ��� ������ ������ �Ÿ� �Ѿ� ���̵� ������ �ȴ�.
            RaycastHit hit;
            ray = new Ray(firePos.position, firePos.forward);
            //source.PlayOneShot(shootClip, 1f);

            SoundManager.instance.playSFX(transform.position, shootClip, false);
            if (Physics.Raycast(ray, out hit, 200f))
            {
                isHit = true;
                laserBeam.FireRay(); //Terrain�� ������ Ray ���
                ShowEffect(hit);
            }
            else
            {
                isHit = false;
                laserBeam.FireRay(); //���� ���� ��쿡�� ����Ʈ�� �ߵ��� �Ѵ�.
                ShowEffect(hit);
            }
        }

        void ShowEffect(RaycastHit hit)
        {
            if (isHit)
            {
                Vector3 hitPoint = hit.point; //Ray�� ���� ��ġ
                Vector3 _normal = (firePos.position - hitPoint).normalized; //����
                Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
                var effect = Instantiate(expEffect, hitPoint, rot);
                StartCoroutine(PlayExplosionClip(0.15f, hitPoint));
                Destroy(effect, 1.5f);
            }
            else
            {
                Vector3 hitPoint = ray.GetPoint(200f);
                Vector3 _normal = (firePos.position - hitPoint).normalized; //����
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
