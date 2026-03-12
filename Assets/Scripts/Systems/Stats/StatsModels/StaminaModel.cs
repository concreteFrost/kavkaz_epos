using UnityEngine;

[System.Serializable]   
public class StaminaModel : ResourceStatModel
{

    protected override float PerLevelBonus => 10f;
    protected override float DiminishFactor => 0.85f;

    public StaminaModel(float baseStamina, float minRegenDelay=0, float maxRegenDelay = 0, float rate = 0)
	{
        statType = global::StatType.Stamina;
        modelType = global::ModifiedModelType.Stamina;  

        BaseInit(baseStamina, minRegenDelay, maxRegenDelay, rate);
        
    }

    //public override void ReduceCurrent(float amount)
    //{
    //    if (Current <= 0) return;

    //    Current -= amount;
    //    RegenTimer = 0;

    //    NotifyCurrentChange(Current);

    //}

    public override void ChangeCurrent(float amount, OperationType operationType)
    {
        float delta = operationType == OperationType.Positive ? amount : -amount;

        if (delta > 0 && Current >= CurrentMax) return;
        if (delta < 0 && Current <= 0) return;

        Current += delta;
        Current = Mathf.Clamp(Current, 0, CurrentMax);

        if (delta < 0)
            RegenTimer = 0;

        NotifyCurrentChange(Current);

    }


}
