using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    Transform tr;//��Ȳ�� �°� ������ �̹��� ũ�Ⱑ ���ϵ��� �� ���̹Ƿ�
    public Image crosshair; //CrossHair�� ������ Ŭ������� ���Ƽ� ��������.
    private float startTime; //�������� Ŀ���� �����ϴ� �ð� ���� ����� - ���Ž� = �귯�� �ð�
    public float duration = 0.2f; //������ Ŀ���� �ӵ�
    public float minSize = 0.8f; //�ּ� ũ��
    public float maxSize = 1.2f; //�ִ� ũ��
    //�ʱ� ����
    private Color originColor = new Color(1f, 1f, 1f, 0.8f);
    public Color gazeColor = Color.red; //gaze: �����ϴ�, �ٶ󺸴�., Enemy�� �ܴ� �� ����
    public bool isGaze = false; //������ �����ߴ°� T/F
    // Start is called before the first frame update
    void Start()
    {
      crosshair = GetComponent<Image>();
        tr = transform;
        startTime = Time.time;
        tr.localScale = Vector3.one * minSize;
        //������ ũ��� �ּ� �������̴�. Vector3.one =  x,y,z�� ũ�Ⱑ ������ ������ �ǵ��� �Ѵ�. 2D���ӿ��� ���� ���δ�.
        crosshair.color = originColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGaze) //������ ������ ��Ҵٸ�
        {
            float t = (Time.time - startTime) / duration; //����� - ���Ž� = �귯�� �ð�       �귯�� �ð�/0.2f
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t); //�ּ� ũ�⿡�� �ִ� ũ��� t��ŭ ���������� Ŀ����.
            crosshair.color = gazeColor;
        }
        else //������ ������ ���� �ʾҴٸ�
        {
            tr.localScale = Vector3.one * minSize;
            crosshair.color = originColor;
            startTime = Time.time;
        }
    }
}
