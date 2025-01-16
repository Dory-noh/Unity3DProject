using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// hp가 감소 damage 동작 애니메이션 구현 Die 동작 애니메이션 
public class SkeletonDamage : MonoBehaviour
{
    public int hp = 0;
    public int hpInit = 100;
    Vector3 hpBarOffset = new Vector3(0, 2.5f, 0);
    public Animator animator;
    public bool isDie = false;
    public Image hpBar;
    public GameObject hpCanvas;
    public BoxCollider swordCol;
    public AudioSource source;
    public AudioClip swordClip;
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

        hpCanvas = PoolingManager.P_Instance.GetHpBar();
        hpBar = hpCanvas.transform.GetChild(0).GetComponent<Image>();
        hpCanvas.SetActive(true);
        var _hpBar = hpCanvas.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = gameObject.transform;
        _hpBar.offset = hpBarOffset;
        HealthBar();
    }
    private void OnCollisionEnter(Collision col)
    {
        int damage = (int)GameObject.FindWithTag("Player").transform.GetComponent<FireBullet>().damage;
        if (!weaponChange.isHaveM4A1) damage *= 2;
        if (col.gameObject.tag =="BULLET")
        {
            //Destroy(col.gameObject);//총알이 사라짐 
            col.gameObject.SetActive(false);  
            hp -= damage;
            hp = Mathf.Clamp(hp, 0, 100);
            HealthBar();
            animator.SetTrigger("Hit");
            if (hp <= 0)
                Die();
        }

    }

    private void HealthBar()
    {
        hpBar.fillAmount = (float)hp / (float)hpInit;
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow;
    }

    void Die()
    {
        if (isDie) return;
        animator.SetTrigger("Die");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDie = true;
        //hpCanvas.enabled = false;
        hpCanvas.SetActive(false);
        GameManager.Instance.Killscore(1); //매니저는 무조건 싱글톤으로 만들어야 한다.
        Destroy(gameObject, 5f);
    }

    public void swordEnable()
    {
        source.PlayOneShot(swordClip, 1.0f);
        swordCol.enabled = true;
    }
    public void swordDisable()
    {
        swordCol.enabled=false;
    }
}
