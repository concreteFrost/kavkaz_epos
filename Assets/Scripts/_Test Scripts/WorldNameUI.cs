using TMPro;
using UnityEngine;

public class WorldNameUI : MonoBehaviour
{

    LastBonfire teleport;
    TextMeshPro text_worldName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        text_worldName = GetComponent<TextMeshPro>();
    }
    void Start()
    {
        teleport = GetComponentInParent<LastBonfire>();
       

        if(teleport != null)
        {
            text_worldName.text = teleport.GetDestinationName();
        }
    }

    
}
