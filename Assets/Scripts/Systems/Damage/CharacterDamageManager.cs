using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterDamageManager : MonoBehaviour, IDamagable
{
    [SerializeField] protected string damagableId;
    [SerializeField] protected float currentHealth;

    public string SourceId() => damagableId;

    public string SelfId() => damagableId;

    public float Health() => currentHealth;

    public abstract void TakeDamage(float damage);

    public void Die()
    {
        Debug.Log("died");
    }

   
}
