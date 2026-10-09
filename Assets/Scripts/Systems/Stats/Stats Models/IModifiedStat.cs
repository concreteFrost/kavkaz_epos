public interface IModifiedStat
{

    ModifiedModelType ModifiedModelType();
    void ChangeCurrent(float val, OperationType type);
    void ChangeMax(string id, float val, OperationType type);
    void ChangeRegenRate(string id, float val, OperationType type);

    void ResetMax(string id);
    void ResetRegenRate(string id);
}