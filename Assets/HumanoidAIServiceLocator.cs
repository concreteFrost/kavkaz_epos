using UnityEngine;

public class HumanoidAIServiceLocator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;
    private HumanoidAIAnimator animatorController = new HumanoidAIAnimator();


    private void Awake()
    {
        animatorController.Init(motor, animator);

        
        motor.Init(animator);
        controller.Init(motor, animator, animatorController);
    }
}
