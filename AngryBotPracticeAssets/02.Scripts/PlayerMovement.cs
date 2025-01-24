using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController CC;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hpPanel;
    [SerializeField] Image hpBar;
    [SerializeField] PlayerInput_m playerInput_m;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    [SerializeField] Vector3 OriginPos;

    private float moveSpeed = 0f;
    private float walkSpeed = 5f;
    private float runSpeed = 9f;
    private float rotSpeed = 40f;

    private float hp = 0f;
    private readonly float MaxHp = 100f;
    private bool isDie = false;
    private readonly int hashRespawn = Animator.StringToHash("Respawn");
    void Start()
    {
        CC = GetComponent<CharacterController>();
        firePos = transform.GetChild(2).GetChild(0).transform;
        bulletPrefab = Resources.Load<GameObject>("Bullet");
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
       source = firePos.GetComponent<AudioSource>();
        clip = Resources.Load<AudioClip>("gun");
        playerInput_m = GetComponent<PlayerInput_m>();
        moveSpeed = walkSpeed;
        OriginPos = transform.position;
        hpPanel = transform.GetChild(5).gameObject;
        hpBar = transform.GetChild(5).GetChild(1).GetComponent<Image>();
        isDie = false;
        hp = MaxHp;
        hpBar.fillAmount = (float)hp / MaxHp;
        hpBar.color = Color.green;
        isDie = false;
    }
    
    void Update()
    {
        if (GameManager.Instance.isGameover == true && GameManager.Instance != null) return;
        if (isDie) return;
        Debug.DrawRay(firePos.position, transform.forward, Color.red);
        moveSpeed = playerInput_m.isRun == false ? walkSpeed : runSpeed;
        MoveAndRotate();
        
    }

    private void MoveAndRotate()
    {
        Vector3 moveVelocity = (transform.right * playerInput_m.PosX + transform.forward * playerInput_m.PosY) * moveSpeed;
        if (CC.isGrounded == false) moveVelocity += Physics.gravity;
        //Debug.Log(moveSpeed);
        CC.Move(moveVelocity * Time.deltaTime);
        CC.transform.Rotate(Vector3.up, playerInput_m.RotX * Time.deltaTime * rotSpeed);
    }

    public void OnDamaged()
    {
        if (isDie) return;
        Debug.Log(gameObject.name + " " + hp);
        hp -= 10f;
        hp = Mathf.Clamp(hp, 0, MaxHp);
        ChangehpBar();
    }

    private void ChangehpBar()
    {
   
        hpBar.fillAmount = (float)hp / MaxHp;
        if (hpBar.fillAmount <= 0.3f)
        {
            hpBar.color = Color.red;
        }
        else if (hpBar.fillAmount <= 0.5f)
        {
            hpBar.color = Color.yellow;
        }
        else
        {
            hpBar.color = Color.green;
        }
        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        
        GameManager.Instance.isGameover = true;
        isDie = true;
        playerInput_m.OnDie();
        CC.enabled = false;
        
        GetComponent<Animator>().SetBool(hashRespawn, false);
        yield return new WaitForSeconds(3f);
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(4).GetComponent<SkinnedMeshRenderer>().enabled = false;
        hpPanel.SetActive(false);
        transform.position = OriginPos;
        GetComponent<Animator>().SetBool(hashRespawn, true);
        yield return new WaitForSeconds(1f);
        
        CC.enabled = true;
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(4).GetComponent<SkinnedMeshRenderer>().enabled = true;
        isDie = false;
        hp = MaxHp;
        hpBar.fillAmount = (float)hp / MaxHp;
        hpBar.color = Color.green;
        isDie = false;
        
        GameManager.Instance.isGameover = false;
        hpPanel.SetActive(true);
    }

    public void FireBullet()
    {
        if (muzzleFlash.isPlaying == false) muzzleFlash.Play();
            source.PlayOneShot(clip, 1f);
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
