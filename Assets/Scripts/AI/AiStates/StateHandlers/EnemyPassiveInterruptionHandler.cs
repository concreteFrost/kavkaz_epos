using UnityEngine;

[System.Serializable]   
public class EnemyPassiveInterruptionHandler
{

    [SerializeField] private bool isInterrupted = false;
    Vector3 interruptionPosition;
    
    private float interruptionTimer=0f;
    private float maxInterruptionTimer = 5f;    

    public bool IsInterrupted() => isInterrupted;

    public void OnDamageTaken(Transform attackSource) {

        if (attackSource == null) return;    
        
        interruptionPosition = attackSource.position;
        isInterrupted = true;

    }

    /// <summary>
    /// Обновляет таймер отвлеченности и сбрасывает до изначального состояния
    /// </summary>
    public void HandleInterruptionUpdate()
    {
        if (!isInterrupted) return;

        interruptionTimer += Time.deltaTime;

        if (interruptionTimer >= maxInterruptionTimer)
        {
            interruptionTimer = 0f;
            isInterrupted = false;
        }

    }
    
    /// <summary>
    /// Возвращает позицию отвлекающего. Используется для того
    /// чтобы агент мог отправиться на позицию отвлекающего
    /// </summary>
    /// <returns></returns>
    public Vector3 InterruptorPosition()=> interruptionPosition;


    /// <summary>
    /// Определяет реакцию на отвлечение. 
    /// </summary>
    /// <param name="selfPosition"></param>
    /// <param name="anim"></param>
    /// <returns></returns>
    public AIStateResult React(Vector3 selfPosition,Animator anim)
    {

       
        float dist = Vector3.Distance(interruptionPosition, selfPosition);

        //если цель дальше трёх метров то следовать к источнику отвлечения
        if (dist > 3f)
        {
            return AIStateResult.MoveToInterruptor;
        }

        //если цель близка то просто проиграть анимацию осматривания по сторонам
        anim.CrossFade(AnimatorParameters.lookAroundState, 0f, 0);
        return AIStateResult.None;


    }
}
