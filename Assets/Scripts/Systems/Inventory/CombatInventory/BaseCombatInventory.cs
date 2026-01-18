using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCombatInventory : MonoBehaviour , IAttackSource
{
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;


    private Transform sourcePosition;

    protected IHumanoidCombat combatController;
  

    #region IAttackSource Contract
    public int SourceId() => sourceId;

    public Transform SourcePosition() => sourcePosition;    

    public List<CharacterType> TargetsToIgnore { get => targetsToIgnore; set => targetsToIgnore = value; }

    public Transform GetRightHand() => rightHand;
    public Transform GetLeftHand() => leftHand;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } = null;

    public IShield ShieldWeapon { get; set; } = null;


    #endregion

    /// <summary>
    /// Уникальный идентификатор нападающего
    /// </summary>
    private int sourceId; 

    /// <summary>
    /// Цели для игнорирования во время атаки
    /// </summary>
    public List<CharacterType> targetsToIgnore = new List<CharacterType>();

    public virtual void Init(HumanoidCombatInventoryService service)
    {
        sourceId = service.sourceId;
        sourcePosition = service.sourcePosition;

        combatController = service.combatController;

      


    }



}
