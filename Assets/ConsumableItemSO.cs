public abstract class ConsumableItemSO<T> :UsableItemSO<T>
{
    public StatusEffectData effectData;
    public abstract override void UseItem(T ctx );

    public AnimationInfoSO consumableAnimation;
}





