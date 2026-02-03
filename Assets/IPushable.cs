using UnityEngine;

public enum PushDirection
{
    Back = 0, 
    Forward = 1   
}
public interface IPushable
{
    CharacterType CharacterType { get; set; }
    void GetPushed(PushDirection dir);
}
