using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FireMissile : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ApacheAI apacheAI;
    [SerializeField] LineRenderer a_FirePos1 = null;
    [SerializeField] LineRenderer a_FirePos2 = null;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] AudioClip explosionClip = null;
    int tankLayer;
    float curDelay = 0f;
    readonly float maxDelay = 1f;

    void Start()
    {
        apacheAI = GetComponent<ApacheAI>();
        a_FirePos1 = GetComponentsInChildren<LineRenderer>()[0];
        a_FirePos2 = GetComponentsInChildren<LineRenderer>()[1];
        tankLayer = LayerMask.NameToLayer("TANK");
        curDelay = maxDelay;
        hitEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        explosionClip = Resources.Load<AudioClip>("Sounds/enemy_Spider_DestroyedExplosion");
    }

    void Update()
    {
        switch (apacheAI.state)
        {
            case ApacheAI.ApacheState.ATTACK:
                FireLaserBeam();
                break;
        }
    }

    void FireLaserBeam()
    {
        Ray ray1 = new Ray(a_FirePos1.transform.position, a_FirePos1.transform.forward);
        Ray ray2 = new Ray(a_FirePos2.transform.position, a_FirePos2.transform.forward);

        RaycastHit hit;
        if(Physics.Raycast(ray1, out hit, Mathf.Infinity, 1<<tankLayer) 
            || Physics.Raycast(ray2, out hit, Mathf.Infinity, 1 << tankLayer))
        {
            curDelay -= 0.01f;
            if (curDelay < 0)
            {
                curDelay = maxDelay;
                a_FirePos1.GetComponent<LaserBeam>().FireRay();
                a_FirePos2.GetComponent<LaserBeam>().FireRay();
                ShowEffect(hit);
            }
        }
    }
    void ShowEffect(RaycastHit hit)
    {
        Vector3 hitPos = hit.point;
        Vector3 hitNormal = (a_FirePos1.transform.position - hitPos).normalized; //πÊ«‚
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal);

        GameObject hitEff = Instantiate(hitEffect, hitPos, rot);
        SoundManager.instance.playSFX(hitPos, explosionClip, false);
        Destroy(hitEff, 0.5f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else if (stream.IsReading)
        {

        }
    }
}
