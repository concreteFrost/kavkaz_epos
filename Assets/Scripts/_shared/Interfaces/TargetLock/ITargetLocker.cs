using System;

public interface ITargetLocker
{
    bool IsLockedOnTarget { get; }
}