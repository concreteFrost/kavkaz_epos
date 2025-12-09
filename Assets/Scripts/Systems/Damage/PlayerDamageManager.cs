using UnityEngine;

public class PlayerDamageManager : CharacterDamageManager
{
    IAttackSource inventory;  
    ICharacterAnimator animator;
    private void Awake()
    {
        damagableId = GetInstanceID().ToString();
    }

    public void Init(IAttackSource src, ICharacterAnimator anim)
    {
        inventory = src;
        animator = anim;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20, Random.Range(0,1f));
        }

    }

    public override void TakeDamage(float damage, float balanceDamage)
    {
      
        animator.BalancePenalty = balanceDamage;

        if (!animator.IsShieldRaised)
        {
            animator.IsDamaged = true;
        }
       
        currentHealth -= damage ;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

  


}
