using System.Collections;
using UnityEngine;


public class PlayerDamageController : MonoBehaviour, IDamagable
{
    IHumanoidCombat combatController;
    IAttackSource inventory;
    
    CharacterStatsController statsController;
    CharacterStats stats;
    PlayerInput input;
    
    protected string uniqueID;

    protected bool isDead;

    #region Damage variables
    protected bool isDamaged;
    protected bool canTakeAnotherDamage = true;
    protected float balancePenalty;

    [SerializeField] protected float maxDamageCooldown = 1f; //предотвращает повторное получение урона
    #endregion

    #region IDamagable Contract
    public bool IsDead() => isDead;
    public string SourceId() => uniqueID;
    public bool IsDamaged { get => isDamaged; set => isDamaged = value; }
    public float BalancePenalty { get => balancePenalty; set => balancePenalty = value; }
    #endregion

    public void Init(CharacterStatsController statsController,CharacterStats stats ,IHumanoidCombat combatController, IAttackSource inventory, PlayerInput input, string uniqueID)
    {
        this.uniqueID = uniqueID; 
        this.statsController = statsController;
        this.combatController = combatController;
        this.inventory = inventory;
        this.input = input; 
        this.stats = stats;

        stats.Health.Depleted += Die;
    }


    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20, UnityEngine.Random.Range(0, 1f), null);
        }
    }

    public void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
        if (isDead || !canTakeAnotherDamage) return;

        balancePenalty = balanceDamage;

        if (!combatController.IsShieldRaised)
        {
            isDamaged = true;
        }

        statsController.ReduceHealth(damage);   
        StartCoroutine(DamageCooldownCoroutine(maxDamageCooldown));

    }
    IEnumerator DamageCooldownCoroutine(float delay)
    {
        canTakeAnotherDamage = false;
        yield return new WaitForSeconds(delay);
        canTakeAnotherDamage = true;
    }


    public void Die()
    {
        isDead = true;

        input.controls.Player.Disable();

        inventory.CurrentWeapon?.DropWeapon();
        inventory.ShieldWeapon?.ThrowShield();
        inventory.ResetWeapon();

        StartCoroutine(RespawnCoroutine(5f));
    }

    public void Respawn()
    {

        input.controls.Player.Enable();
        statsController.ResetAllStats();

        isDead = false;

    }

    private IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }
}
