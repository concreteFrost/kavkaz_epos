using UnityEngine;

[System.Serializable]
public class BalanceModel : ResourceStatModel
{
    private float balanceRecoverDelay = 2f;
    private float balanceRecoverRate = 20f;

    private float lastHitTime;
    public BalanceModel(float baseBalance)
    {
        statType = global:: StatType.Balance;
        modelType = global::ModifiedModelType.Balance;

        BaseInit(baseBalance, balanceRecoverDelay,balanceRecoverDelay,balanceRecoverRate);
    }

    public override void Regen()
    {
        if (Time.time - lastHitTime < balanceRecoverDelay)
            return;

       base.Regen();
    }

    public bool ApplyBalanceDamage(float damage)
    {
        lastHitTime = Time.time;

        Current -= damage;

        NotifyCurrentChange(Current);

        if (Current <= 0)
        {
            Current = CurrentMax;
            NotifyCurrentChange(Current);
            return true;
        }

        return false;
    }
}