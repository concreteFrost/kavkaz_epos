public interface IStatModifier
{
    ModifiedModelType ModifiedModelType();   
    public void IncreaseCurrent(float val);

    public void ReduceCurrent(float val); 
}