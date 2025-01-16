using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//isGaze가 True이면(적에게 닿았는지 EyeCast2통해 체크) 크기를 1.2배로 하고 색을 빨강으로 바꾼다.
//원본은 흰색,alpha=0.8f
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
        if (isGaze) // 적에 광선이 닿았을 때
        {
            CrossHair.color = Color.red;
            float t = Time.time - beginTime / duration;
            //조준점이 특정 속도로 부드럽게 커지도록 한다.
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, t);
        }
        else //적에 광선이 안 닿았을 때
        {
            CrossHair.color = originColor;
            tr.localScale = Vector3.one * minSize;
            beginTime = Time.time;
        }
    }
}
