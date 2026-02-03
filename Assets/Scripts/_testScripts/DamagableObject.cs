using System;
using System.Collections;
using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{

    public CharacterType characterType;

    [SerializeField] float currentHealth = 30;
    [SerializeField] string selfId;

    Color defaultCol;
    MeshRenderer mat;

    #region IDamagable Contract
    public CharacterType CharacterType { get => characterType; set => characterType = value; }
    public Transform GetAimTransform() => transform;
    public Transform GetOrigin() => transform;
    public bool IsDead { get; set; }
    public string SourceId() => selfId;
    public bool IsDamaged { get; set; }
    public bool IsKnockedOut {  get; set; } 
    public BalanceDamageType BalancePenalty {  get; set; }

    public event Action<Transform> DamageTaken = null;

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

        characterType = CharacterType.Object;
    }

    public void TakeDamage(DamageData damageData,Transform source)
    {
        if (IsDead) return;
        currentHealth -= damageData.healthDamageMultiplier;
        StartCoroutine(DamageCoroutine());

        if (currentHealth <= 0)
        {

            Die();
        }

        DamageTaken?.Invoke(source);
    }


    public virtual void Die()
    {
        IsDead = true;
        gameObject.SetActive(false);
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

    public void GetPushed(PushDirection direction)
    {
        //без реализации
    }

    public void CancelPush()
    {
        //без реализации
    }
}
