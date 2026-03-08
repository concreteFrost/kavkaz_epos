using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Instant Status Effect Item" , fileName ="Instant Status Effect Item")]
public class InstantStatModifierItemSO: ConsumableItemSO {

    public StatusEffectData effectData;
    public void UseItem(CharacterStatsModifier ctx)
    {
        ctx.ApplyInstantSideEffect(effectData);
    }

}








