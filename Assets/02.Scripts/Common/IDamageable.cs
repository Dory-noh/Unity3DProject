using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal); //혈흔이 맞은 위치에서 맞은 방향으로 튀게 하기 위해 메서드의 매개변수로 넣어주었다.
}
