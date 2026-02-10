
using UnityEngine;
public class DoorLock : Lock
{

    public override void LockAction()
    {
        Debug.LogWarning($"[DoorLock] LockAction called but door is null on {gameObject.name}");

    }

    public override void UnlockAction()
    {
        Debug.LogWarning($"[DoorLock] UnlockAction called but door is null on {gameObject.name}");
    }
}