using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankInput : MonoBehaviourPun
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
        //���� ������ �Ǹ�
        if (GameManager.Instance != null && GameManager.Instance.isGameOver && !photonView.IsMine) //����䰡 �� ���� �ƴϸ� ���� �� ��
        {
            //���� ����� �ʱ�ȭ�Ѵ�.
            rotate = 0f;
            move = 0f;
            m_ScrollWheel = 0f;
            isFire = false;
            return;
        }
        //update������ ���ڿ� ���� �Ҵ��� ���� �ʰ� ���ڿ� �´��� �񱳸� �ϰ� �ϱ� ���� ���� readonly�� �̿��� ���� ������ ���־���.
        rotate = Input.GetAxis(horizontal); //a, dŰ�� ���� ȸ����Ų��.
        move = Input.GetAxis(vertical); //w, sŰ�� ���� ����, ���� �۵���Ų��.
        m_ScrollWheel = Input.GetAxis(m_scrollWheel);
        isFire = Input.GetButtonDown(fire1);
    }
}
