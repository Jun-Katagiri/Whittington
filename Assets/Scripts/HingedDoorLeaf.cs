using UnityEngine;

public class HingedDoorLeaf : DoorLeaf
{
    [SerializeField] Transform pivot;
    [SerializeField] float openAngle = 90f;
    [SerializeField] float speed = 6f;

    bool hasCachedRotations;
    Quaternion closedRot;
    Quaternion openRot;

    void Awake()
    {
        if (!pivot) pivot = transform;
        CacheRotations();
        if (IsOpen)
            pivot.localRotation = openRot;
    }

    void Update()
    {
        if (!pivot) pivot = transform;
        if (!hasCachedRotations) CacheRotations();

        var target = IsOpen ? openRot : closedRot;
        if (speed <= 0f)
        {
            pivot.localRotation = target;
            return;
        }

        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, target, Time.deltaTime * speed);
    }

    public override void Open()
    {
        if (!pivot) pivot = transform;
        if (!hasCachedRotations) CacheRotations();
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    void CacheRotations()
    {
        closedRot = pivot ? pivot.localRotation : transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
        hasCachedRotations = true;
    }
}
