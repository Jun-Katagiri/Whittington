using UnityEngine;

public class Door : MonoBehaviour, DoorInteractable
{



    protected bool isOpen;


    void Awake()
    {
    }

    void Update()
    {
    }

    public void Interact()
    {
        Toggle();
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open() => isOpen = true;
    public void Close() => isOpen = false;


}

