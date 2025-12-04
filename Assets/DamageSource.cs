using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] DamageCollider damageCollider;

    public float cooldown = 2f;
    float defaultCooldown;

    bool colliderActive = false;

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
            damageCollider.EnableCollider(10);
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
