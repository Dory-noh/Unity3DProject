//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerAndEnemyConnect : ScriptableObject
//{
//    public static PlayerAndEnemyConnect connect;
//    public FireBullet fireBullet;
//    public EnemyFire enemyFire;

//    private readonly string playerTag = "Player";
//    private readonly string enemyTag = "ENEMY";
//    void Start()
//    {
//        connect = this;
//        fireBullet = GameObject.FindWithTag(playerTag).GetComponent<FireBullet>();
//        enemyFire = GameObject.FindWithTag(enemyTag).GetComponent<EnemyFire>();
//    }
//}
