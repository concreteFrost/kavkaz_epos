using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidAITester : MonoBehaviour
{
    public HumanoidAIMotor aiMotor;
    public HumanoidAIController controller;
    public EnemyFOVController targetLock;
    public HumanoidCombatController combatController;
    public HumanoidCombatInventory inventory;
    public CharacterInteract interaction;
    public Animator anim;
    public Transform targetPoint;

    private bool isMovingToTarget;
    private bool isMovingToDefault;
    private bool isComboRunning;
    private Weapon targetWeapon;
    private Shield targetShield;

    public float weaponCheckRadius = 15f;


    Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;
    }


    void Update()
    {
        if (aiMotor == null) return;
        if (!aiMotor.agent.enabled) return;

        if (isMovingToTarget) MoveToTarget();
        if (isMovingToDefault) MoveToDefaultPosition();

        if (targetWeapon != null) GoToWeapon();
        if(targetShield != null) GoToShield();  


    }

    #region Movement
    public void MoveToTarget()
    {
        isMovingToTarget = true;
        isMovingToDefault = false;

        if (NavMesh.SamplePosition(targetPoint.position, out var hit, 1f, NavMesh.AllAreas))
        {
            aiMotor.MoveCharacter(hit.position);
            float distance = Vector3.Distance(hit.position, transform.position);

            if (distance < 1f)
            {
                aiMotor.StopMovement();
                isMovingToTarget = false;
            }
        }
        else
        {
            Debug.Log("path is invalid");
            aiMotor.StopMovement();
            isMovingToTarget = false;
        }
    }

    public void MoveToDefaultPosition()
    {
        isMovingToDefault = true;
        isMovingToTarget = false;

        aiMotor.MoveCharacter(defaultPosition);
        aiMotor.ResetLockTarget();

        float distance = Vector3.Distance(defaultPosition, transform.position);

        if (distance < 1f)
        {
            aiMotor.StopMovement();
            isMovingToDefault = false;

        }

    }

    public void Dodge()
    {
        if (!targetLock.IsLockedOnTarget) return;

        aiMotor.agent.ResetPath();

        // направление ОТ цели
        Vector3 fromTarget = (transform.position - targetPoint.position).normalized;

        //// локальные направления
        //Vector3 back = fromTarget;
        //Vector3 left = Quaternion.AngleAxis(-90f, Vector3.up) * fromTarget;
        //Vector3 right = Quaternion.AngleAxis(90f, Vector3.up) * fromTarget;

        //int choice = Random.Range(0, 3);

        //Vector3 chosenDir = choice switch
        //{
        //    0 => back,
        //    1 => left,
        //    _ => right
        //};

        //// переводим в локальное пространство персонажа
        //Vector3 localDir = transform.InverseTransformDirection(chosenDir);

        //Vector2 dodgeInput = new Vector2(localDir.x, localDir.z).normalized;

        aiMotor.Dodge(fromTarget);

    }

    #endregion

    #region Target Lock

    public void SetLockOnTarget()
    {
        //var t =  targetLock.CheckNearestTarget();

        //Debug.Log(t);
        //if(t != null) 
        //controller.SetLockTarget(t);

    }

    public void ResetLockTarget()
    {
        aiMotor.ResetLockTarget();
    }

    #endregion
    #region Combat
    public void SingleAttack()
    {
        combatController.PerformAttack();
    }

    public void Combo()
    {
        if (isComboRunning) return;

        bool willThrowWeapon = Random.value > 0.9f;

        if (willThrowWeapon)
        {
            combatController.ThrowWeapon();
        }
        else
        {

            int punchesCount = Random.RandomRange(3, 7);
            StartCoroutine(ComboSampleCoroutine(punchesCount));
        }

    }

    IEnumerator ComboSampleCoroutine(int punchesCount)
    {
        isComboRunning = true;
        int executedAttacks = 0;

        

        Debug.Log(punchesCount);

        void OnAttackEnd()
        {
            executedAttacks++;
        }

        combatController.OnAttackEnd += OnAttackEnd;

        // первый инпут
        SingleAttack();
      
           

        while (executedAttacks < punchesCount-1)
        {
            // ждём окно буфера
            yield return new WaitForSeconds(combatController.attackBufferTime * 0.9f);
            SingleAttack();
        }

        combatController.OnAttackEnd -= OnAttackEnd;
        isComboRunning = false;
    }

    #endregion

    #region Interaction

    public void FindWeapon()
    {
        isMovingToDefault = false;
        isMovingToTarget = false;
        targetShield = null;

        Collider[] cols = Physics.OverlapSphere(transform.position, weaponCheckRadius);

        foreach (Collider col in cols) { 
        
            if(col.GetComponent<Weapon>() != null){

                var weapon = col.GetComponent<Weapon>();

                if(weapon.AttackSource == null)
                {
                    targetWeapon = weapon;
                    Debug.Log("found weapon");
                }
                    
                
            }
        }
        
    }

    public void FindWShield()
    {
        isMovingToDefault = false;
        isMovingToTarget = false;
        targetWeapon = null;    

        Collider[] cols = Physics.OverlapSphere(transform.position, weaponCheckRadius);

        foreach (Collider col in cols)
        {

            if (col.GetComponent<Shield>() != null)
            {

                var weapon = col.GetComponent<Shield>();

                if (weapon.AttackSource == null)
                {
                    targetShield = weapon;
                    Debug.Log("found weapon");
                }


            }
        }

    }

    private void GoToWeapon()
    {
        if (targetWeapon == null) aiMotor.StopMovement();
        if (targetWeapon.AttackSource != null) aiMotor.StopMovement();

        Debug.Log("going to weapon");

        if (NavMesh.SamplePosition(targetWeapon.transform.position, out var hit, 1f, NavMesh.AllAreas))
        {
            aiMotor.MoveCharacter(targetWeapon.transform.position);

            
            float distance = Vector3.Distance(transform.position, targetWeapon.transform.position);

        

            if (distance < 0.5f)
            {
                aiMotor.StopMovement();
                interaction.Interact();
                targetWeapon = null;
            }
        }
        else
        {
            Debug.Log("path is invalid");
            aiMotor.StopMovement();
            targetWeapon = null;
        }

    }

    private void GoToShield()
    {
        if (targetShield == null) aiMotor.StopMovement();
        if (targetShield.AttackSource != null) aiMotor.StopMovement();

        Debug.Log("going to shield");

        if (NavMesh.SamplePosition(targetShield.transform.position, out var hit, 1f, NavMesh.AllAreas))
        {
            aiMotor.MoveCharacter(targetShield.transform.position);


            float distance = Vector3.Distance(transform.position, targetShield.transform.position);

            

            if (distance < 0.5f)
            {
                aiMotor.StopMovement();
                interaction.Interact();
                targetShield = null;
            }
        }
        else
        {
            Debug.Log("path is invalid");
            aiMotor.StopMovement();
            targetShield = null;
        }

    }


    #endregion


}
