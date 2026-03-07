public interface IStatModel
{

    public float CurrentMax { get; set; }   
    void ReduceCurrent(float value);    
    void IncreaseCurrent(float value);

}