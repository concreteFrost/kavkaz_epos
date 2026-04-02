using UnityEngine;

[CreateAssetMenu(fileName = "Character Behaviour Stats", menuName = ScriptablePaths.CHARACTER_BEHAVIOUR_STATS_PATH + "/Character Behaviour Stats")]
public class CharacterBehaviourStatsSO : ScriptableObject
{

    [Header("Idle")]
    [Tooltip("Минимальное время (в секундах), которое персонаж стоит без движения в состоянии Idle")]
    [Range(3, 6)]
    public int minIdleStationary;

    [Tooltip("Максимальное время (в секундах), которое персонаж стоит без движения в состоянии Idle")]
    [Range(6, 20)]
    public int maxIdleStationary;

    [Header("Patrol")]
    [Tooltip("Максимальный радиус (в метрах), в пределах которого выбирается точка патрулирования")]
    public float maxDestiantionRadius = 10f;

    [Tooltip("Максимальное количество попыток найти валидную точку для патруля")]
    public int maxPatrolAttempts = 3;

    [Header("Chase")]
    [Tooltip("Время (в секундах), в течение которого цель считается недостижимой")]
    public float maxCantReachTimer = 7f;

    [Tooltip("Максимальное время (в секундах), после которого цель считается потерянной")]
    public float maxLostTargetTimer = 10f;

    [Tooltip("Максимальная дистанция, на которой начинается преследование цели")]
    public float maxChaseDistance = 17f;

    [Tooltip("Время ожидания (в секундах) перед сменой поведения во время преследования")]
    public float maxWaitTimer = 7f;

    [Header("Combat Distance Settings")]
    public CombatDistancesSO combatDistancesSO;

    [Header("Combat")]

    [Tooltip("Вероятность перехода в атаку (0–1)")]
    public float attackTransitionChance = .8f;

    [Tooltip("Начальный шанс выполнить мощную атаку (0–1)")]
    public float initialPoweAttackChance = 0.15f;

    [Tooltip("Множитель увеличения шанса мощной атаки со временем")]
    public float powerAttackChanceMultiplier = 0.05f;

    [Tooltip("Минимальная задержка между атаками (в секундах)")]
    public float minCombatCooldown = 0.2f;

    [Tooltip("Максимальная задержка между атаками (в секундах)")]
    public float maxCombatCooldown = 0.7f;

    [Tooltip("Начальный шанс уклонения (0–1)")]
    [Range(0, 1f)]
    public float initialDodgeChance = 0.2f;

    [Tooltip("Множитель увеличения шанса уклонения")]
    public float dodgeChanceMultiplier = 0.15f;

    [Header("Combat distances")]
    [Tooltip("Дистанция, на которой происходит переход из преследования в боевое состояние. Для рукопашника =3, для мага=10")]
    public float fromChaseToCombatDistance = 10f;

    [Tooltip("Максимальная дистанция, на которой персонаж остается в бою")]
    public float maxDistanceInCombat = 8f;

    [Tooltip("Дистанция на которой включается стрейф")]
    public float targetLockOnDistance = 2f;
    //[Tooltip("Дистанция, при которой персонаж переключается на бег во время боя. Оптимальное значение = 3")]
    //public float switchToRunDistance = 3f;

    //[Tooltip("Дистанция, на которой возможна обычная атакаю. Оптимальное значение = 1.3")]
    //public float meleeDistance = 1.3f;

    //[Tooltip("Дистанция каста. Оптимальное значение = 10")]
    //public float spellCastDistance = 10f;

    [Header("Strafe")]
    [Tooltip("Максимальная дистанция до цели, при которой разрешено стрейфиться")]
    public float maxTargetDistanceInStrafe = 10f;

    [Tooltip("Минимальное время (в секундах) нахождения в состоянии стрейфа")]
    [Range(3, 7)]
    public int minTimeInStrafeState = 7;

    [Tooltip("Максимальное время (в секундах) нахождения в состоянии стрейфа")]
    [Range(7, 12)]
    public int maxTimeInStrafeState = 12;

    [Header("Wait Behaviour")]
    [Tooltip("Шанс (0–1), что персонаж решит переместиться в состоянии ожидания цели")]
    [Range(0f, 1f)]
    public float willMoveChance = 0.25f;

    [Tooltip("Минимальная задержка перед сменой позиции (в секундах)")]
    public float minRepositionCooldown = 2f;

    [Tooltip("Максимальная задержка перед сменой позиции (в секундах)")]
    public float maxRepositionCooldown = 4f;

    [Header("Mixed Combat")]
    [Tooltip("Выгода от рукопашной атаки на основе расстояния от цели")]
    public float meleeScore = 2f;

    [Tooltip("Выгода от магической атаки на основе расстояния от цели")]
    public float spellScore = 10f;

}