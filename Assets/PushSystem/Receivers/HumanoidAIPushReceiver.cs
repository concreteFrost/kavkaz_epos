using UnityEngine;

public class HumanoidAIPushReceiver : BasePushReceiver
{
    IDamagable damageController;
    BaseHumanoidAnimatorController animatorController;

    IRagdollController ragdollController;
    PushDirection pushedDirection;

    public void Init(
        IDamagable damageController,
        BaseHumanoidAnimatorController animatorController,
        IRagdollController ragdollController,
        Transform self
        )
    {
        this.damageController = damageController;
        this.animatorController = animatorController;    
        this.ragdollController = ragdollController;

        this.self = self;

        characterType = damageController.CharacterType;
    }

  
    public override void CancelPush()
    { 
        IsPushed = false;
    }

    public override void GetPushed(PushDirection dir, Transform source)
    {
        if (IsPushed || damageController.IsDead) return;

        animatorController.GetPushed(dir);
        pushedDirection = dir;

        IsPushed = true;

        InvokePushReceived(source);
    }

    public override void TrackPush()
    {
        if (!IsPushed) return;

        // базовая точка — центр персонажа
        Vector3 origin = self.position;
        origin.y += 0.2f; // немного выше ног

        // определяем смещение по направлению толчка
        float offset = 0.7f;
        Vector3 offsetDir = pushedDirection == PushDirection.Forward ? -self.forward : self.forward;
        Vector3 checkPoint = origin + offsetDir * offset;

        // проверка земли под точкой
        if (Physics.Raycast(checkPoint, Vector3.down, out RaycastHit hitInfo, 1.5f))
        {
            return; // есть земля — ничего не делаем
        }

        // передаём в ragdollController: Transform остаётся внутри, сила считается по fallDirection
        ragdollController.Knockout(transform.position - offsetDir, 500);

        IsPushed = false;
    }




}



