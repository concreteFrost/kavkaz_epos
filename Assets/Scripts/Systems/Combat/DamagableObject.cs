using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{
    private bool isDead;

    [SerializeField] float currentHealth = 30;
    [SerializeField] string selfId;

    public bool IsDead() => isDead;
    public string SourceId() => selfId;
  
    public float Health() => currentHealth;
    public void TakeDamage(float damage, float balanceDamage)
    {
        if(isDead) return;  

        Debug.Log(damage + " " + balanceDamage);
        currentHealth -= damage;

        if (currentHealth <= 0) { 
        
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    private void Awake()
    {
        selfId = GetInstanceID().ToString();
    }

}
