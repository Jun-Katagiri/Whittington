using UnityEngine;

public class InwardOnlyDoor : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float openAngle = 90f; // inward direction (fixed)
    [SerializeField] float speed = 6f;

    bool isOpen;
    Quaternion closedRot;
    Quaternion openRot;

    void Awake()
    {
        if (!pivot) pivot = transform;
        closedRot = pivot.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        var target = isOpen ? openRot : closedRot;
        pivot.localRotation = Quaternion.Slerp(
            pivot.localRotation,
            target,
            Time.deltaTime * speed
        );
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }
    
    public void Toggle_d()
    {
        Debug.Log($"[Door] Toggle called on {name}, pivot={(pivot ? pivot.name : "null")}, isOpen={isOpen}");
        isOpen = !isOpen;
    }


    public void Open() => isOpen = true;
    public void Close() => isOpen = false;
}