using System;
using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]
    [SerializeField] private MeleeData meleeData;

    public void Init(
        CharacterBoneSocket boneSocket,
        BaseHumanoidAnimatorController animatorController,
        IHumanoidMeleeCombat combatController,
        ICollector collector,
        bool enableWeaponBreakdown)
    {
        
        this.boneSocket = boneSocket;
        this.combatController = combatController;
        this.animatorController = animatorController;
        this.Collector = collector; 
        this.enableWeponBreakdown = enableWeaponBreakdown;


        DefaultWeapon = InitializeBarehands(collector);
        SetWeapon(DefaultWeapon);

    }

    private IWeapon InitializeBarehands(ICollector attackSource)
    {

        var bareHands = new MeleeWeapon();
        bareHands.Init(meleeData, attackSource);
        return bareHands;

    }

}

