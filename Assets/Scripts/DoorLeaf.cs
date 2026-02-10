using UnityEngine;

public abstract class DoorLeaf : MonoBehaviour
{
    public abstract void Open();
    public abstract void Close();

    public abstract void Toggle();

    public abstract bool IsOpen { get; }
}
