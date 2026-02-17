using UnityEngine;


public class HumanoidAIPushReceiver : BasePushReceiver
{
    IDamagable damageController;
    BaseHumanoidAnimatorController animatorController;

    IRagdollController ragdollController;
    PushDirection pushedDirection;
    Transform pushSource;

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
        InvokePushReceived(pushSource);
        pushSource = null;
    }

    public override void GetPushed(PushDirection dir, Transform source)
    {
        if (IsPushed || damageController.IsDead) return;

        animatorController.GetPushed(dir);
        pushedDirection = dir;
        pushSource = source;

        IsPushed = true;
        

       
    }

    public override void TrackPush()
    {
        if (!IsPushed) return;

        // базовая точка — центр персонажа
        Vector3 origin = self.position;
        origin.y += 0.1f; // немного выше ног

        // определяем смещение по направлению толчка
        float offset = 1f;
        Vector3 offsetDir = pushedDirection == PushDirection.Forward ? -self.forward : self.forward;
        Vector3 checkPoint = origin + offsetDir * offset;

        // проверка земли под точкой
        if (Physics.Raycast(checkPoint, Vector3.down, out RaycastHit hitInfo, 1.5f))
        {
            return; // есть земля — ничего не делаем
        }

        // передаём в ragdollController: Transform остаётся внутри, сила считается по fallDirection
        ragdollController.Knockout(transform.position - offsetDir, 500);

        damageController.IsKnockedOut = true;
        IsPushed = false;
    }




}



