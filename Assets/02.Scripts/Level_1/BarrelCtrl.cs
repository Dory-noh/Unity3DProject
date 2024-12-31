using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject expEffect; //explosion effect
    [SerializeField] private Mesh[] meshes; //폭파 후 찌그러짐을 표현하는 Mesh 배열
    [SerializeField] private Texture[] textures; //Barrel을 랜덤하게 표현할 Texture
    [SerializeField] private AudioSource source; //Audio Source Component of Barrel Object
    [SerializeField] private AudioClip expClip; //Barrel Explosion Sound Clip
    public int hitCount = 0;
    [SerializeField] MeshRenderer _renderer;//Mesh Renderer
    [SerializeField] MeshFilter meshFilter; //Mesh Filter
    public float expRadius = 15; //Expolsion Range
    Vector3 pos = Vector3.zero;

    private readonly string bulletTag = "BULLET";
    void Start()
    {
        expEffect = Resources.Load<GameObject>("Effect/BulletImpactFleshBigEffect");
        meshes = Resources.LoadAll<Mesh>("BarrelMeshes");
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        source = GetComponent<AudioSource>();
        expClip = Resources.Load<AudioClip>("Sounds/grenade_exp2");
        _renderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag(bulletTag))
        {
            if(++hitCount == 3)
            {
                pos = transform.position;
                ExpBarrel(); //총알이 해당 Barrel에 세 번 충돌하면 해당 Barrel 폭파 //총알의 위치-Barrel의 부딪힌 위치를 ExpBarrel 메서드에  전달
            }
        }
    }
    void ExpBarrel()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); //해당 Barrel의 위치에 이펙트를 회전없이 생성시킨다.
        Destroy(effect, 3.0f);
        //Destroy(gameObject);

        //중점과 반지름으로 가상의 원을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 반환하는 함수 함수의 반환 값은 Collider 컴포넌트의 배열로 넘어옵니다.
        Collider[] Cols = Physics.OverlapSphere(pos, expRadius, 1<<8);
        //폭파 위치에서 15 반경 이내에 Collider를 갖고 있는 오브젝트들을 배열로 반환하는 메서드
        foreach (Collider col in Cols)
        {
            var rb = col.GetComponent<Rigidbody>();
            if(rb != null) //유효성 검사
            {
                rb.mass = 1.0f; //폭파 시 무게를 줄인다.
                rb.AddExplosionForce(1200f, pos, expRadius, 1000f);
                //폭파력 위치   반경   위로 솟구치는 힘
            }
        }
        source.PlayOneShot(expClip, 1f);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];//Barrel에 찌그러진(찌그러진 위치가 다른 메시들이 들어있는 배열임) 메시가 랜덤으로 들어간다.
        GetComponent<MeshCollider>().sharedMesh = meshes[idx]; //collider도 meshfilter 모양대로 찌그러지도록 한다.
        Invoke("BarrelMass", 3f);
    }

    void BarrelMass()
    {
        //중점과 반지름으로 가상의 원을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 반환하는 함수 함수의 반환 값은 Collider 컴포넌트의 배열로 넘어옵니다.
        Collider[] Cols = Physics.OverlapSphere(pos, expRadius, 1 << 8);
        //폭파 위치에서 15 반경 이내에 Collider를 갖고 있는 오브젝트들을 배열로 반환하는 메서드
        foreach (Collider col in Cols)
        {
            var rb = col.GetComponent<Rigidbody>();
            if (rb != null) //유효성 검사
            {
                rb.mass = 60.0f; //폭파 시 무게를 줄인다.
                //폭파력 위치   반경   위로 솟구치는 힘
            }
        }
    }
    void Update()
    {
        
    }
}
