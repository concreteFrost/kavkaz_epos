using UnityEngine;

public interface ITargetLockable
{
	Transform GetTargetTransform();
	bool IsActive();
	void SetTargetActive(bool active);	
}
