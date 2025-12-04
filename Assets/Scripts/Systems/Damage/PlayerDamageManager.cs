
public class PlayerDamageManager : CharacterDamageManager
{
    private void Awake()
    {
        damagableId = GetInstanceID().ToString();
    }
}
