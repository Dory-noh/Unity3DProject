using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform firePos;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject effectWall;
    [SerializeField] GameObject effectPlayerOREnemy;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        effectWall = Resources.Load<GameObject>("Bullet_Impact_Wall");
        effectPlayerOREnemy = Resources.Load<GameObject>("Bullet_Impact_Enemy");
        rb.AddRelativeForce(Vector3.forward * 10000);
        Destroy(gameObject, 2.0f);
    }

    void Update()
    {
        if (GameManager.Instance.isGameover == true && GameManager.Instance != null) return;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f, 1 << 8))
        {
            var obj = Instantiate(effectWall, hit.point, Quaternion.identity);
            Destroy(obj, 2.0f);
            Destroy(gameObject,0.05f);
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f, ( 1 << 9)))
        {
            hit.transform.GetComponent<EnemyMovement>().OnDamaged();
            var obj = Instantiate(effectPlayerOREnemy, hit.point, Quaternion.identity);
            Destroy(obj, 2.0f);
            Destroy(gameObject, 0.05f);
        }
    }
}
