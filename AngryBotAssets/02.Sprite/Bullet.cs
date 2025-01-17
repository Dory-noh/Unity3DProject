using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
        Destroy(gameObject, 3.0f);
    }

    private void OnCollisionEnter(Collision col)
    {
        var contact = col.GetContact(0);
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));
        Destroy(obj, 2.0f); //����Ʈ�� 2�� �� �����
        Destroy(this.gameObject); //�Ѿ��� �ٷ� �����
    }
}
