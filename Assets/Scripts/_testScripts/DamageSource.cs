
using UnityEngine;
using System.Collections.Generic;

public class DamageSource : MonoBehaviour
{
    [SerializeField] DamageCollider damageCollider;

    public float cooldown = 2f;
    float defaultCooldown;

    bool colliderActive = false;

    private float healthDamage = 10f;

    public List<CharacterType> objectsToIgnore = new List<CharacterType>(); 

    void Start()
    {
        defaultCooldown = cooldown;
    }

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (!colliderActive)
        {
            // включаем коллайдер
            damageCollider.EnableCollider(healthDamage,BalanceDamageType.Low, objectsToIgnore);
            colliderActive = true;
        }

        if (cooldown <= 0f)
        {
            // выключаем
            damageCollider.DisableCollider();

            // таймер по новой
            cooldown = defaultCooldown;
            colliderActive = false;
        }
    }
}
