using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] InwardOnlyDoor door;

    void Awake()
    {
        if (!door) door = GetComponentInParent<InwardOnlyDoor>();
    }

    public void Interact()
    {
        if (door) door.Toggle();
    }
}
