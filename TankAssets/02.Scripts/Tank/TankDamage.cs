using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TankDamage : MonoBehaviourPun
{
    private MeshRenderer[] renderers;
    private readonly string TankTag = "TANK";
    private Rigidbody rb;
    private GameObject expEffect = null;
    private readonly int InitHp = 100;
    private int CurHp = 0;
    public int playerId = -1;
    [SerializeField] FireCannon fireCannon;
    private WaitForSeconds ws = new WaitForSeconds(5f);
    [SerializeField] Image hpBar;

    void Start()
    {
        fireCannon = GetComponent<FireCannon>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        CurHp = InitHp;
        expEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        hpBar.color = Color.green;
    }

    public void OnDamage(string Tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("OnDamageRPC",RpcTarget.All,fireCannon.playerid);
        }
    }

    [PunRPC] //같이 공유하기 위해서 
    void OnDamageRPC(string Tag)
    {
        if (CurHp > 0 && Tag == TankTag)
        {
            CurHp -= 25;
            if(CurHp <= 0)
            {
                StartCoroutine(explosionTank());
                SetTankVisible(false);
            }
        }
    }

    IEnumerator explosionTank()
    {
        var eff = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(eff, 2f);

        yield return ws;
        CurHp = InitHp;
        SetTankVisible(true);
    }
    void SetTankVisible(bool isVisible)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = isVisible;
        }
    }
}
