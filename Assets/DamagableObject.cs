using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{

    [SerializeField] float currentHealth = 30;
    [SerializeField] string selfId;

    public string SourceId() => selfId;
    public void Die()
    {
       Destroy(gameObject);
    }

    public float Health() => currentHealth;
  

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) { 
        
            Die();
        }
    }

    private void Awake()
    {
        selfId = GetInstanceID().ToString();
    }

}
