using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "GunData", order = 0)]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float fireRate = 0.12f; //발사 간격
    public float reloadTime = 1.8f; //재장전 소요 시간
}
