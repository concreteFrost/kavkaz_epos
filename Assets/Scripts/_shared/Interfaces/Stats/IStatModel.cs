public interface IStatModel
{
    public int CurrentLevel();

    public float CurrentMax { get; set; }   
    public StatType StatType();


}