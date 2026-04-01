using UnityEngine;

public class HumanoidAiInteractionController : BaseCharacterInteractor
{
    public void Init(string collectorId, Transform self,
       CharacterStatsController statsController,
       CharacterStatsModifier statsModifier,
       BaseHumanoidAnimatorController animatorController,
       IWeaponSetter combatInventory,
       IDamagable damageController,
       IAttackSource attackSource,
       ICharacterLifeCycle lifeCycle
      )
    {
        BaseInit(collectorId, self, statsController,statsModifier, animatorController, combatInventory, damageController, attackSource, lifeCycle);
       
    }

    public override void DistributeItemToInventory(ItemData data)
    {
        //
    }
}
