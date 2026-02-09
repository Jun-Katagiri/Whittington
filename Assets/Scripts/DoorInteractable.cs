public interface DoorInteractable : Interactable
{
    public void Open();
    public void Close();

    public void Toggle();

    public void Interact()
    {
        Toggle();
    }



}
