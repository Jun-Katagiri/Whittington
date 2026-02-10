using UnityEngine;

public class Lock : MonoBehaviour
{
    public virtual void LockAction()
    {
        Debug.LogWarning($"[Lock] LockAction called but not overridden on {gameObject.name}");
    }

    public virtual void UnlockAction()
    {
        Debug.LogWarning($"[Lock] UnlockAction called but not overridden on {gameObject.name}");
    }
}