using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInput : MonoBehaviour
{
    public float rotate = 0f, move = 0f, m_ScrollWheel = 0f;
    public bool isFire = false;
    public readonly string horizontal = "Horizontal";
    public readonly string vertical = "Vertical";
    public readonly string m_scrollWheel = "Mouse ScrollWheel";
    public readonly string fire1 = "Fire1";
    void Start()
    {
        
    }

    void Update()
    {
        //게임 오버가 되면
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            //게임 종료시 초기화한다.
            rotate = 0f;
            move = 0f;
            m_ScrollWheel = 0f;
            isFire = false;
            return;
        }
        //update에서는 문자열 동적 할당은 하지 않고 문자열 맞는지 비교만 하게 하기 위해 위에 readonly를 이용한 변수 선언을 해주었다.
        rotate = Input.GetAxis(horizontal); //a, d키를 눌러 회전시킨다.
        move = Input.GetAxis(vertical); //w, s키를 눌러 전진, 후진 작동시킨다.
        m_ScrollWheel = Input.GetAxis(m_scrollWheel);
        isFire = Input.GetButtonDown(fire1);
    }
}
