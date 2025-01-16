using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//isGaze�� True�̸�(������ ��Ҵ��� EyeCast2���� üũ) ũ�⸦ 1.2��� �ϰ� ���� �������� �ٲ۴�.
//������ ���,alpha=0.8f
public class CrossHair2 : MonoBehaviour
{
    public Image CrossHair;
    private float minSize = 0.8f;
    private float maxSize = 1.2f;
    private Color originColor = new Color(1f, 1f, 1f, 0.8f);
    private float duration = 0.2f;
    private float beginTime;
    public bool isGaze = false;
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        CrossHair = GetComponent<Image>();
        CrossHair.color = originColor;
        tr.localScale = Vector3.one * minSize;
        beginTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGaze) // ���� ������ ����� ��
        {
            CrossHair.color = Color.red;
            float t = Time.time - beginTime / duration;
            //�������� Ư�� �ӵ��� �ε巴�� Ŀ������ �Ѵ�.
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t);
        }
        else //���� ������ �� ����� ��
        {
            CrossHair.color = originColor;
            tr.localScale = Vector3.one * minSize;
            beginTime = Time.time;
        }
    }
}
