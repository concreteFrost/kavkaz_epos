using UnityEngine;

public static class ScriptablePaths
{
    //корень
    public const string BASE_PATH = "Scriptable Systems";

    //домены
    public const string COMBAT_PATH = BASE_PATH + "/Combat";
    public const string ITEMS_PATH =BASE_PATH + "/Item";
    public const string CHARACTER_PATH =BASE_PATH + "/Character";
    public const string SENSES_PATH =BASE_PATH + "/Sense";
    public const string ANIMATION_PATH = BASE_PATH + "/Animation";
    public const string PROJECTILE_PATH = BASE_PATH + "/Projectile";
    public const string VFX_PATH = BASE_PATH + "/VFX";
    public const string UI_STYLES_PATH = BASE_PATH + "/UI Styles";
    public const string STATUS_FX_PATH = BASE_PATH + "/Status Effects";
    public const string LOOT_PATH = BASE_PATH + "/Loot";
    public const string BIOM_PATH = BASE_PATH + "/Bioms";
   
    //под-домены
    public const string WEAPON_ATTACK_PATH = COMBAT_PATH + "/Weapon Attacks";

    public const string ITEM_STAT_PATH = ITEMS_PATH + "/Item Stats";

    public const string CHARACTER_STATS_PATH = CHARACTER_PATH + "/Stats";
    public const string CHARACTER_LEVEL_PATH = CHARACTER_PATH + "/Level";
    public const string CHARACTER_BEHAVIOUR_STATS_PATH = CHARACTER_PATH + "/Behaviour Stats";
    public const string CHARACTER_BEHAVIOUR_PROFILES = CHARACTER_PATH + "/Behaviour Profiles";

    public const string CONSUMABLE_ITEM_PATH = ITEMS_PATH + "/Consumable Items";
   
    public const string PROJECTILE_ATTACK_PATH = PROJECTILE_PATH + "/Attack";
    public const string PROJECTILE_INSTANCE_PATH = PROJECTILE_PATH + "/Instances";
    public const string PROJECTILE_MOVE_PATH = PROJECTILE_PATH + "/Movement";

    public const string VFX_SIDE_FX_PATH = VFX_PATH + "/Side Effects Data";



}
