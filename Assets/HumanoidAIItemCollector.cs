using UnityEngine;

public class HumanoidAIItemCollector : BaseItemCollector
{
    public void Init(Transform self,
       CharacterStatsController statsController,
       BaseHumanoidAnimatorController animatorController,
       ICombatInventory combatInventory,
       IDamagable damageController,
       IAttackSource attackSource
      )
    {
        BaseInit(self, statsController, animatorController, combatInventory, damageController, attackSource);
       
    }

    public override void DistributeItemToInventory(ItemData data)
    {
        //
    }
}
