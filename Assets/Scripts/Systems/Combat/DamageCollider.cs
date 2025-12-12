using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider col;
    public List<Collider> collectedColliders = new List<Collider>();

    public bool attackInterrupted = false; // используется при обнаружении щита у цели
    protected float healthDamage;
    protected float balanceDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void EnableCollider(float _healthDamage, float _balanceDamage)
    {
        col.enabled = true;
        healthDamage = _healthDamage;
        balanceDamage = _balanceDamage;
    }

    public void DisableCollider()
    {
        col.enabled = false;
        attackInterrupted = false;
        collectedColliders.Clear();
    }

    //Метод расчета урона с учетом защиты цели
    protected float DamageReductionAmount(Collider other)
    {
        if (other.GetComponent<DefenceCollider>() != null)
        {
            var defence = other.GetComponent<DefenceCollider>();

            float finalDamage = defence.CalculateDamage(healthDamage,balanceDamage);
            return finalDamage;
        }

        return 0;
    }

    //Метод нанесения урона цели
    protected void PerformDamage(Collider other , float _healthDamage)
    {
        var damagable = other.GetComponentInChildren<IDamagable>()
              ?? other.GetComponent<IDamagable>();

        if (damagable == null) return;

        damagable.TakeDamage(healthDamage,balanceDamage);
    }

    //Метод обработки расчета урона с учетом защиты цели 
    protected void HandleDamageCalculation(Collider other, float _healthDamage)
    {
        float finalDamage = DamageReductionAmount(other);

        if (DamageReductionAmount(other) > 0) //щит сработал
        {
            attackInterrupted = true;
            return;
        }

        PerformDamage(other, finalDamage);
    }

    //Метод обработки столкновения коллайдера урона с целью
    protected virtual void HandleCollision(Collider other)
    {
        //Если атака была прервана щитом, то не наносим урон
        if (attackInterrupted)
            return;


        //Если цель уже была поражена этой атакой, то не наносим урон повторно
        if (collectedColliders.Contains(other))
            return;

        collectedColliders.Add(other);

       HandleDamageCalculation(other, healthDamage);

    }

 

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }




}
