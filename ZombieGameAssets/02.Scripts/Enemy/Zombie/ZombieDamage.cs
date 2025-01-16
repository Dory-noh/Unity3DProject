using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ZombieDamage : MonoBehaviour
{
    public int hp = 0;
    public int hpInit = 100;
    public Animator animator;
    public bool isDie = false;
    public Image hpBar;
    public GameObject hpCanvas;
    Vector3 hpBarOffset = new Vector3(0, 3f, 0);
    public BoxCollider Collider;
    public WeaponChange weaponChange;

    void Start()
    {
        hp = hpInit;
        
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

    // 콜라이더가 감지 하면 호출 되는 함수 
    private void OnCollisionEnter(Collision col)
    {
        int damage = (int)GameObject.FindWithTag("Player").transform.GetComponent<FireBullet>().damage;
        if (!weaponChange.isHaveM4A1) damage *= 2;
        if (col.gameObject.tag =="BULLET")
        {

            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            hp -= damage;
            hp = Mathf.Clamp(hp, 0, 100);
            HealthBar();

            animator.SetTrigger("Hit");
        }
        if (hp <= 0)
            Die();
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
        Debug.Log($"HP: {hp} 사망!");
        animator.SetTrigger("Die");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDie = true;
        GameManager.Instance.Killscore(1);
        hpCanvas.SetActive(false);
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
