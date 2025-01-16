using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStop : MonoBehaviour
{
    public GameObject rainPrefab;
    public GameObject rainObj;
    // Start is called before the first frame update
    void Start()
    {
        rainObj = Instantiate(rainPrefab);
        //���� �Ҵ�
        //Hierarchy���� RainPrefab ������Ʈ���� ã�Ƽ� �����Ѵ�.
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        rainPrefab.SetActive(false);
    //    }
    //} //�ʹ� Ŀ�� ����/Ż�� ������ ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(rainObj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rainObj = Instantiate(rainPrefab);
        }
    }
}
