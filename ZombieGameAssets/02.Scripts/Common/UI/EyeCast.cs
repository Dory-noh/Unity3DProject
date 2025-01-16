using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray;//���� �ڷ��� // ����ü
    private RaycastHit hit;
    public CrossHair2 crosshair;
    //� ������Ʈ�� ������ �浹 ������ ��ġ ���� �Ÿ� ���� ��ȯ�ϴ� �ڷ���(����ü)�̴�.

    void Start()
    {
        crosshair = GameObject.Find("Image-CrossHair").GetComponent<CrossHair2>(); //hierarchy���� ������Ʈ���� �̿��� ������Ʈ�� ã�� �� ������Ʈ�� ���� Ŭ������ �����Ѵ�.
        tr = transform; //�ٿ� ���� ���� �̷��� ��
    }


    void Update()
    {
                         //��ġ       //����
        ray = new Ray(tr.position, tr.forward);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue);
        //�浹 ���� �߳���?
        if(Physics.Raycast(ray, out hit, 20f, 1<<6 | 1<<7 | 1<<8)) //out: ��� ���� �Ű����� //�޼��� �����ε�: ������ ray�� ��ġ�� ������ ������ ������ �� �ۼ��� �ʿ� ����. //bit������ : ������ 1�� ����, ������ �ϳ��� �ִ´�.(| &)
        {
            crosshair.isGaze = true;
        }
        //�浹 ���� ���ߴٸ�..
        else
        {
            crosshair.isGaze = false;
        }
    }
}
