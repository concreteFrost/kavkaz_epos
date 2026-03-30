using System;
using UnityEngine;

public interface ICharacterLifeCycle
{
    void Die();
    void Respawn();
    void SetStartingPosition(Vector3 pos);
    void ResetPosition();
}