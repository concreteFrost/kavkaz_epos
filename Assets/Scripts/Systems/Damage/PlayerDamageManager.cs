
using UnityEngine;

public class PlayerDamageManager : CharacterDamageManager
{
    IAttackSource inventory;    
    private void Awake()
    {
        damagableId = GetInstanceID().ToString();
    }

    public void Init(IAttackSource src)
    {
        inventory = src;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20);
        }
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log(damage);
        currentHealth -= damage ;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
