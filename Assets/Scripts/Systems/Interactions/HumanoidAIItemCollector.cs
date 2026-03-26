using UnityEngine;

public class HumanoidAIItemCollector : BaseItemCollector
{
    public void Init(string collectorId, Transform self,
       CharacterStatsController statsController,
       BaseHumanoidAnimatorController animatorController,
       IWeaponSetter combatInventory,
       IDamagable damageController,
       IAttackSource attackSource
      )
    {
        BaseInit(collectorId, self, statsController, animatorController, combatInventory, damageController, attackSource);
       
    }

    public override void DistributeItemToInventory(ItemData data)
    {
        //
    }
}
