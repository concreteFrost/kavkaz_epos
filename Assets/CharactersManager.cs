using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public List<BaseHumanoidAiServiceLocator> characters = new List<BaseHumanoidAiServiceLocator>();

    public void Init()
    {
        characters.Clear();
        characters.AddRange(GetComponentsInChildren<BaseHumanoidAiServiceLocator>());

        foreach (BaseHumanoidAiServiceLocator locator in characters)
        {
            locator.Init();
        }
    }

}
