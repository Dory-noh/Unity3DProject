using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/E2_GunData", fileName = "E2_GunData", order =3 )]
public class E2_GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float fireRate = 0.12f; //�߻� ����
    public float reloadTime = 1.8f; //������ �ҿ� �ð�
}
