using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 생명체는 데미지를 받으면 체력이 깎이고 체력이 0이 되면 사망 기능이 있어 사망 event를 제공한다.
//체력 회복 아이템을 얻으면 체력 회복이 된다.
// LivingEntity는 모든 생명체의 부모 클래스가 된다. -> 메서드들에 virtual 붙임
public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float InitHealth = 100f; //protected 해당 클래스를 상속받은 클래스만 해당 변수에 접근할 수 있다.
    public float Health { get; protected set; } //프로퍼티를 만들었다.
    public bool dead {  get; protected set; } //수정은 자식 클래스에서만 가능하다.
    public event Action onDeath; //사망했을 때 실행될 이벤트이다.

    protected virtual void OnEnable(){ //virtual: 상속으로 물려줄 메서드에는 virtual 키워드를 붙인다. //OnEnable():오브젝트가 활성화될 때 자동으로 실행되는 CallBack메서드
        dead = false;
        Health = InitHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;
        if(Health <= 0 && !dead)
        {
            Die();
        }
    }
    public virtual void RestoreHealth(float newHealth) //체력 회복 아이템을 획득했을 때 실행될 함수
    {
        if(dead) return;
        newHealth += InitHealth;
    }
    public virtual void Die()
    {
        if(onDeath != null) //이벤트가 null이 아니면 onDeath-사망했을 때 실행되는 이벤트 실행
        {
            onDeath(); //사망 트리거 발동
        }
        dead = true;
    }
    
}
