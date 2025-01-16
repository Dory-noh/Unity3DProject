using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDamage : MonoBehaviour
{
    int hp = 0;
    int hpInit = 100;
    public Animator animator;
    public bool isDie = false;

    public Image hpBarImage;
    Vector3 hpBarOffset = new Vector3(0, 2.5f, 0);
    public GameObject hpBar;
    public BoxCollider Collider;
    public WeaponChange weaponChange;
    void Start()
    {
        hp = hpInit;
        animator = GetComponent<Animator>();
        weaponChange = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponChange>();
        
    }
    private void OnEnable()
    {
        StartCoroutine(SetHpBar());
    }
    IEnumerator SetHpBar()
    {
        yield return new WaitForSeconds(0.3f);

        hpBar = PoolingManager.P_Instance.GetHpBar();
        hpBarImage = hpBar.transform.GetChild(0).GetComponent<Image>();
        hpBar.SetActive(true);
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = gameObject.transform;
        _hpBar.offset = hpBarOffset;
        HealthBar();
    }
    public void OnCollisionEnter(Collision col)
    {
        int damage = (int)GameObject.FindWithTag("Player").transform.GetComponent<FireBullet>().damage;
        if (!weaponChange.isHaveM4A1) damage *= 2;
        if (col.gameObject.tag == "BULLET")
        {
            col.gameObject.SetActive(false);
            hp -= damage;
            hp = Mathf.Clamp(hp, 0, 100);
            HealthBar();
            animator.SetTrigger("Hit");
            if (hp <= 0)
            {
                Die();
            }
        }
        
    }

    private void HealthBar()
    {
        hpBarImage.fillAmount = (float)hp / (float)hpInit;
        if (hpBarImage.fillAmount <= 0.3f)
        {
            hpBarImage.color = Color.red;
        }
        else if (hpBarImage.fillAmount <= 0.5f)
        {
            hpBarImage.color = Color.yellow;
        }
    }

    private void Die()
    {
        if (isDie) return;
        animator.SetTrigger("Die");
        isDie = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        //hpCanvas.enabled = false;
        GameManager.Instance.Killscore(1);
        hpBar.SetActive(false);
        Destroy(gameObject, 5f);
    }

    public void EnableCollider()
    {
        Collider.enabled = true;
    }

    public void DisableCollider()
    {
        Collider.enabled = false;
    }
}
