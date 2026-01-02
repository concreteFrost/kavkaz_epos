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
    public void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
        if(isDead) return;  
        currentHealth -= damage;
        StartCoroutine(DamageCoroutine());

        if (currentHealth <= 0) { 
        
            Die();
        }
    }


    public virtual void Die()
    {
        isDead = true;
        gameObject.SetActive(false);    
    }

    public bool IsDamaged { get; set; }
    public float BalancePenalty {  get; set; }   

    #endregion

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        selfId = GetInstanceID().ToString();
        mat = GetComponent<MeshRenderer>();
        defaultCol = mat.material.color;
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
