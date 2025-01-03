using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/E_GunData", fileName = "E_GunData", order =1 )]
public class E_GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float fireRate = 0.12f; //발사 간격
    public float reloadTime = 1.8f; //재장전 소요 시간
}
