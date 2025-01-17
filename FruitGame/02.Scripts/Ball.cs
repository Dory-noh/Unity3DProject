using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] ballPrefabs;
    [SerializeField] bool isColliding = false;
    void Start()
    {
        isColliding = false;
        ballPrefabs = Resources.LoadAll<GameObject>("Balls");
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col) //���� �±� �� ������Ʈ���� �浹 �� ���ʿ��� �޼��尡 ����Ǿ �ߺ� �����Ǵ� ���� �߻�
    {
        if (col.transform.CompareTag(transform.tag))
        {
            Debug.Log(col.transform.name +" " +transform.gameObject.name);
            if(gameObject.GetInstanceID() < col.gameObject.GetInstanceID()) StartCoroutine(HandCollisionDelay(col));
        }
    }
    private IEnumerator HandCollisionDelay(Collision2D col)
    {
        yield return new WaitForSeconds(0.1f);
        //if(col!=null && transform != null)
        {
            if (!col.transform.CompareTag("Floor")&&col.gameObject!=null) Destroy(transform.gameObject);
            if(!col.transform.CompareTag("Floor")&&gameObject!=null) Destroy(col.gameObject);
            GameObject ball = Instantiate(ballPrefabs[int.Parse(transform.tag)], (transform.position+col.transform.position)/2, transform.rotation);
            ball.name = ballPrefabs[int.Parse(transform.tag)].name;
        }
    }
}
