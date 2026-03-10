using System.Numerics;

public interface IBreakable
{
    float GetDurability();
    bool IsBreakdownEnabled { get; set; }
    bool IsBroken { get; set; }
    void SetBreakdownEnabled(bool isEnabled);
    void Break();
    void Recover();

    public void ReduceDurability(float amount);

    public void IncreaseDurability(float amount);
}