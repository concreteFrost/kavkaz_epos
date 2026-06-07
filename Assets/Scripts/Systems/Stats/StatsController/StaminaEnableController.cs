using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetHolder
{
    public CharacterType targetType;
    public string fovId;
}
public class StaminaEnableController : MonoBehaviour
{

    public List<TargetHolder> targetHolders = new List<TargetHolder>();
    StaminaModel staminaModel;

    public void Init(CharacterStatsController statsController)
    {
        staminaModel = statsController.Stamina;
        staminaModel.canReduceStamina = false;
        EnemyFOVController.TargetFound += OnTargetAdded;
        EnemyFOVController.TargetLost += OnTargetReset;

    }

    private void OnDisable()
    {
        EnemyFOVController.TargetFound -= OnTargetAdded;
        EnemyFOVController.TargetLost -= OnTargetReset;
    }
    private void OnTargetAdded(CharacterType type, string fovId)
    {
        if(type == CharacterType.Player)
        {
            TargetHolder holder = new TargetHolder()
            {
                targetType = type,
                fovId = fovId
            };

            targetHolders.Add(holder);
            staminaModel.canReduceStamina = true;
        }
    }

    private void OnTargetReset(CharacterType type, string fovId)
    {
        if(type == CharacterType.Player)
        {
            var match = targetHolders.Find(x => x.fovId == fovId);  

            if(match != null)
            {
                targetHolders.Remove(match);    
            }

            if(targetHolders.Count == 0)
            {
                staminaModel.canReduceStamina = false;
            }
        }
    }
}
