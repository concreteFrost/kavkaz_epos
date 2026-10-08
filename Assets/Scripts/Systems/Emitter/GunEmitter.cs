using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEmitter : Emitter, IAttackSource
{
    [SerializeField] CharacterType type;
    [SerializeField] private ProjectileSO targetProjectileSO;

    public int SourceId() => (int)type;

    public Transform Source() => transform;

    [SerializeField] private List<CharacterType> targetsToIgnore;
    public List<CharacterType> TargetsToIgnore { get => targetsToIgnore; set => targetsToIgnore = value; }
    private void Awake()
    {
        emitSource = transform;
        attackSource = this;
    }


    public override void StartEmit()
    {
        base.StartEmit();
    }

    public override void Emit()
    {
        projectileSO = targetProjectileSO;
        base.Emit();
    }

    public void EmitOnTarget(IDamagable target)
    {
        SetTargetData(target);
        Emit();
    }

    
}

