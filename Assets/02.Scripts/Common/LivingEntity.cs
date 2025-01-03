using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ����ü�� �������� ������ ü���� ���̰� ü���� 0�� �Ǹ� ��� ����� �־� ��� event�� �����Ѵ�.
//ü�� ȸ�� �������� ������ ü�� ȸ���� �ȴ�.
// LivingEntity�� ��� ����ü�� �θ� Ŭ������ �ȴ�. -> �޼���鿡 virtual ����
public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float InitHealth = 100f; //protected �ش� Ŭ������ ��ӹ��� Ŭ������ �ش� ������ ������ �� �ִ�.
    public float Health { get; protected set; } //������Ƽ�� �������.
    public bool dead {  get; protected set; } //������ �ڽ� Ŭ���������� �����ϴ�.
    public event Action onDeath; //������� �� ����� �̺�Ʈ�̴�.

    protected virtual void OnEnable(){ //virtual: ������� ������ �޼��忡�� virtual Ű���带 ���δ�. //OnEnable():������Ʈ�� Ȱ��ȭ�� �� �ڵ����� ����Ǵ� CallBack�޼���
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
    public virtual void RestoreHealth(float newHealth) //ü�� ȸ�� �������� ȹ������ �� ����� �Լ�
    {
        if(dead) return;
        newHealth += InitHealth;
    }
    public virtual void Die()
    {
        if(onDeath != null) //�̺�Ʈ�� null�� �ƴϸ� onDeath-������� �� ����Ǵ� �̺�Ʈ ����
        {
            onDeath(); //��� Ʈ���� �ߵ�
        }
        dead = true;
    }
    
}
