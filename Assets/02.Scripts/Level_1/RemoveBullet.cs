using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitclip;
    [SerializeField] GameObject hitEffect;
    private readonly string bulletTag = "BULLET";
    private void Start()
    {
        source = GetComponent<AudioSource>();
        hitclip = Resources.Load<AudioClip>("Sounds/bullet_hit_metal_enemy_4");
        hitEffect = Resources.Load<GameObject>("Effect/FlareMobile");
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag(bulletTag))
        {
            source.PlayOneShot(hitclip, 1.0f);
            GameObject hitEff = Instantiate(hitEffect,col.transform.position,col.transform.rotation);
            Destroy(hitEff, 2.0f);
            Destroy(col.gameObject);
        }
    }
}
