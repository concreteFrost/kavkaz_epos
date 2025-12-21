using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] DamageCollider damageCollider;

    public float cooldown = 2f;
    float defaultCooldown;

    bool colliderActive = false;

    private float healthDamage = 10f;
    private float balanceDamage = 0.1f;

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
            damageCollider.EnableCollider(healthDamage,balanceDamage, null);
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
