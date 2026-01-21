using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{

    public ProjectileData data;
    float currLifeTime; //текущий жизненый цикл

    void Update()
    {
        Vector3 velocity = data.Move.Move(
            transform,
            data.target,
            data.baseDir,
            data.speed
        );

        transform.position += velocity * Time.deltaTime;

        currLifeTime += Time.deltaTime;
        if (currLifeTime >= data.lifetime)
            Destroy(gameObject);
    }

    public void Init(ProjectileData data)
    {
        this.data = data;    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            var damagable = other.GetComponent<IDamagable>();
            damagable.TakeDamage(data.damage,BalanceDamageType.Low, null);

        }

        Destroy(gameObject);
    }
}
