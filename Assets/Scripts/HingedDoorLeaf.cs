using UnityEngine;

public class HingedDoorLeaf : DoorLeaf
{
    protected bool isOpen = false;

    public override bool IsOpen => isOpen;

    [SerializeField] Transform pivot;
    [SerializeField] float openAngle = 90f; // inward direction (fixed)
    [SerializeField] float speed = 6f;



    bool hasCachedRotations;
    Quaternion closedRot;
    Quaternion openRot;

    void Awake()
    {
        if (!pivot) pivot = transform;
        CacheRotations();
    }

    void Update()
    {
        if (!pivot) pivot = transform;
        if (!hasCachedRotations) CacheRotations();

        var target = isOpen ? openRot : closedRot;
        if (speed <= 0f)
        {
            pivot.localRotation = target;
            return;
        }

        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, target, Time.deltaTime * speed);
    }

    public override void Toggle()
    {

        // debug log
        Debug.Log($"[HingedDoor] Toggle called on {name}, pivot={(pivot ? pivot.name : "null")}, hingeIsOpen={isOpen}");

        if (isOpen)
            Close();
        else
            Open();
    }




    public override void Open()
    {
        if (!pivot) pivot = transform;

        // Cache once unless explicitly invalidated elsewhere.
        // Re-caching while transitioning can shift the closed baseline.
        if (!hasCachedRotations)
            CacheRotations();

        isOpen = true;
    }
    public override void Close() => isOpen = false;

    void CacheRotations()
    {
        closedRot = pivot ? pivot.localRotation : transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
        hasCachedRotations = true;
    }

}
