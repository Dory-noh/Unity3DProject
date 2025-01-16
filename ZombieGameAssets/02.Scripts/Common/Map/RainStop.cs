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
        //동적 할당
        //Hierarchy에서 RainPrefab 오브젝트명을 찾아서 대입한다.
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        rainPrefab.SetActive(false);
    //    }
    //} //너무 커서 입장/탈출 감지를 못함
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
