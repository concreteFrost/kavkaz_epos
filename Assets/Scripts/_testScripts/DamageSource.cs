
using UnityEngine;
using System.Collections.Generic;

public class DamageSource : MonoBehaviour
{
    [SerializeField] DamageCollider damageCollider;
    public DamageData damageData;
    public float cooldown = 2f;
    float defaultCooldown;

    bool colliderActive = false;

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
            damageCollider.EnableCollider(damageData, objectsToIgnore, null);
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
