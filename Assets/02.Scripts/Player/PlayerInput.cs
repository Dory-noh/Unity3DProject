using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] readonly string strHorzontal = "Horizontal";
    [SerializeField] readonly string strVertical = "Vertical"; //��Ÿ�ӽ� �Ҵ� 
    [SerializeField] readonly string strMouse_X = "Mouse X";
    [SerializeField] readonly string fireBtn = "Fire1";
    [SerializeField] PlayerDamage damage;
    //[SerializeField] readonly string reloadBtn = "Reload";

    public float h = 0, v = 0, r= 0;
    public bool fire { get; private set; } = false;
    public bool sprint { get; private set; } = false;
    //public bool reload { get; private set; } = false;

    void Start()
    {
        damage = GetComponent<PlayerDamage>();
        
    }

    void Update()
    {
        if (damage.isPlayerDie) return;
        if (GameManager.instance != null && GameManager.instance.isGameover) //�ʱ�ȭ�ϰ� ��������
        {
            h = 0f;
            v= 0f;
            r = 0f;
            fire = false;
            sprint = false;
            //reload = false;
            return; //���� ������ �����.
        }
        h = Input.GetAxis(strHorzontal);
        v = Input.GetAxis(strVertical);
        r = Input.GetAxisRaw(strMouse_X); //GetAxisRaw�� �� ������, GetAxis�� �ε巴��.
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            sprint = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = false;
        }
        fire = Input.GetButtonDown(fireBtn);
        //reload = Input.GetButtonDown(reloadBtn);
    }
}
