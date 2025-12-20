using System.Collections;
using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{
    private bool isDead;

    [SerializeField] float currentHealth = 30;
    [SerializeField] string selfId;

    Color defaultCol;
    MeshRenderer mat;

    #region IDamagable Contract
    public bool IsDead() => isDead;
    public string SourceId() => selfId;
    public float Health() => currentHealth;
    public void TakeDamage(float damage, float balanceDamage)
    {
        if(isDead) return;  
        currentHealth -= damage;
        StartCoroutine(DamageCoroutine());

        if (currentHealth <= 0) { 
        
            Die();
        }
    }

    public virtual void Init()
    {
        selfId = GetInstanceID().ToString();
        mat = GetComponent<MeshRenderer>();
        defaultCol = mat.material.color;
    }

    public virtual void Die()
    {
        isDead = true;
        gameObject.SetActive(false);    
    }

    #endregion

    private void Awake()
    {
        Init();
    }

    IEnumerator DamageCoroutine()
    {
        
       
        var col = Color.white;
        var col2 = Color.green;

        float elapsed = 0f;

        while(elapsed < 1f)
        {

            col = Color.Lerp(defaultCol, col2, Mathf.PingPong(elapsed += (Time.deltaTime * 2 / 1),1f));
            mat.material.color = col;   

            elapsed += Time.deltaTime;

            yield return null;
        }

        mat.material.color = defaultCol;

        

    }
}
