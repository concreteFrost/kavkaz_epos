using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterDamageManager : MonoBehaviour, IDamagable
{
    [SerializeField] protected string damagableId;
    [SerializeField] protected float currentHealth;

    public string SourceId() => damagableId;

    public string SelfId() => damagableId;

    public float Health() => currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }


    public void Die()
    {
        Debug.Log("died");
    }

   
}
