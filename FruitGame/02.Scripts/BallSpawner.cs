using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ballPrefabs;
    [SerializeField] Transform tr;
    void Start()
    {
        tr = transform;
        ballPrefabs = Resources.LoadAll<GameObject>("Balls");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RespawnBall();
        }
    }

    void RespawnBall()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 ballPos = new Vector3(mouseWorldPosition.x, tr.position.y, tr.position.z);
        int idx = Random.Range(0, 3);
        GameObject ball = Instantiate(ballPrefabs[idx], ballPos, tr.rotation);
        ball.name = ballPrefabs[idx].name;
    }
}
