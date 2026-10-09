using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class TargetHolder
{
    public CharacterType targetType;
    public string fovId;
}
public class StaminaEnableController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_activeTargets;
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

    private void Update()
    {
       
    }

    private void OnTargetAdded(CharacterType type, string fovId)
    {

        if (type == CharacterType.Player)
        {
            TargetHolder holder = new TargetHolder()
            {
                targetType = type,
                fovId = fovId
            };

            var match = targetHolders.Find(x => x.fovId == fovId);

            if (match == null)
            {
                targetHolders.Add(holder);
                //staminaModel.canReduceStamina = true;
            }

        }

        text_activeTargets.text = "Active targets: " + targetHolders.Count.ToString();

        staminaModel.canReduceStamina = targetHolders.Count != 0;
    }

    private void OnTargetReset(CharacterType type, string fovId)
    {

        if (type == CharacterType.Player)
        {

            var match = targetHolders.Find(x => x.fovId == fovId);

            if (match != null)
            {
                targetHolders.Remove(match);
            }


        }

        text_activeTargets.text = "Active targets: " + targetHolders.Count.ToString();

        staminaModel.canReduceStamina = targetHolders.Count != 0;
    }
}
