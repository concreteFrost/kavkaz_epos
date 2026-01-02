using System;

public class PlayerLifeCicle
{
    PlayerCombatInventory inventory;
    PlayerStats stats;
    PlayerInput input;
    PlayerStatsUI ui;

    public event Action<float> Died;

    public PlayerLifeCicle(PlayerCombatInventory inventory, PlayerStats stats, PlayerInput input, PlayerStatsUI ui)
    {
        this.inventory = inventory;
        this.stats = stats; 
        this.input = input;
        this.ui = ui;
    }
    public void Die()
    {
        //isDead = true;
        //damagable.IsDead() = true;
        input.controls.Player.Disable();

        inventory.CurrentWeapon?.DropWeapon();
        inventory.ShieldWeapon?.ThrowShield();
        inventory.ResetWeapon();

        //Died?.Invoke(5f);
        //StartCoroutine(RespawnCoroutine(5f));

    }

    public void Respawn()
    {

        input.controls.Player.Enable();

        stats.Health.Current = stats.maxHealth;
        stats.Stamina.Current = stats.maxStamina;

        ui.UpdateHealthSlider(stats.Health.Current);
        ui.UpdateHealthSlider(stats.Health.Current);

        //isDead = false;

    }

}
