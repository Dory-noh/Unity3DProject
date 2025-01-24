using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class E_Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject effectWall;
    [SerializeField] GameObject effectPlayerOREnemy;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        effectWall = Resources.Load<GameObject>("Bullet_Impact_Wall");
        effectPlayerOREnemy = Resources.Load<GameObject>("Bullet_Impact_Enemy");
        rb.AddRelativeForce(Vector3.forward * 10000);
        Destroy(gameObject, 1.0f);
    }

    void Update()
    {
        if (GameManager.Instance.isGameover == true && GameManager.Instance != null) return;
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, 1 << 8))
        {
            var obj = Instantiate(effectWall, hit.point, Quaternion.identity);
            Destroy(obj, 2.0f);
            Destroy(gameObject,0.05f);
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, (1 << 7)))
        {
            hit.transform.GetComponent<PlayerMovement>().OnDamaged();
            var obj = Instantiate(effectPlayerOREnemy, hit.point, Quaternion.identity);
            Destroy(obj, 2.0f);
            Destroy(gameObject, 0.01f);
        }
    }
}
