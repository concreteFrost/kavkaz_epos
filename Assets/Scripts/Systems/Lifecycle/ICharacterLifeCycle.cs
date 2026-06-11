using System;
using UnityEngine;

public interface ICharacterLifeCycle
{
    void Die();
    void Respawn(Vector3 pos);
    void SetStartingPosition(Vector3 pos);
    void ResetPosition();
}