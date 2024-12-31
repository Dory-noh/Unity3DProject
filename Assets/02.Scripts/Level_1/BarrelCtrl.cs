using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject expEffect; //explosion effect
    [SerializeField] private Mesh[] meshes; //���� �� ��׷����� ǥ���ϴ� Mesh �迭
    [SerializeField] private Texture[] textures; //Barrel�� �����ϰ� ǥ���� Texture
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
                ExpBarrel(); //�Ѿ��� �ش� Barrel�� �� �� �浹�ϸ� �ش� Barrel ���� //�Ѿ��� ��ġ-Barrel�� �ε��� ��ġ�� ExpBarrel �޼��忡  ����
            }
        }
    }
    void ExpBarrel()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); //�ش� Barrel�� ��ġ�� ����Ʈ�� ȸ������ ������Ų��.
        Destroy(effect, 3.0f);
        //Destroy(gameObject);

        //������ ���������� ������ ���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� ��ȯ�ϴ� �Լ� �Լ��� ��ȯ ���� Collider ������Ʈ�� �迭�� �Ѿ�ɴϴ�.
        Collider[] Cols = Physics.OverlapSphere(pos, expRadius, 1<<8);
        //���� ��ġ���� 15 �ݰ� �̳��� Collider�� ���� �ִ� ������Ʈ���� �迭�� ��ȯ�ϴ� �޼���
        foreach (Collider col in Cols)
        {
            var rb = col.GetComponent<Rigidbody>();
            if(rb != null) //��ȿ�� �˻�
            {
                rb.mass = 1.0f; //���� �� ���Ը� ���δ�.
                rb.AddExplosionForce(1200f, pos, expRadius, 1000f);
                //���ķ� ��ġ   �ݰ�   ���� �ڱ�ġ�� ��
            }
        }
        source.PlayOneShot(expClip, 1f);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];//Barrel�� ��׷���(��׷��� ��ġ�� �ٸ� �޽õ��� ����ִ� �迭��) �޽ð� �������� ����.
        GetComponent<MeshCollider>().sharedMesh = meshes[idx]; //collider�� meshfilter ����� ��׷������� �Ѵ�.
        Invoke("BarrelMass", 3f);
    }

    void BarrelMass()
    {
        //������ ���������� ������ ���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� ��ȯ�ϴ� �Լ� �Լ��� ��ȯ ���� Collider ������Ʈ�� �迭�� �Ѿ�ɴϴ�.
        Collider[] Cols = Physics.OverlapSphere(pos, expRadius, 1 << 8);
        //���� ��ġ���� 15 �ݰ� �̳��� Collider�� ���� �ִ� ������Ʈ���� �迭�� ��ȯ�ϴ� �޼���
        foreach (Collider col in Cols)
        {
            var rb = col.GetComponent<Rigidbody>();
            if (rb != null) //��ȿ�� �˻�
            {
                rb.mass = 60.0f; //���� �� ���Ը� ���δ�.
                //���ķ� ��ġ   �ݰ�   ���� �ڱ�ġ�� ��
            }
        }
    }
    void Update()
    {
        
    }
}
