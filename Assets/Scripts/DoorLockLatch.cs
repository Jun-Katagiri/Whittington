using UnityEngine;
using System.Collections.Generic;

public class DoorLockLatch : MonoBehaviour, Interactable
{
    [SerializeField] bool startsLocked;
    [SerializeField] DoorController door;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lockClip;
    [SerializeField] AudioClip unlockClip;

    public bool IsLocked { get; private set; }

    void Awake()
    {
        IsLocked = startsLocked;
        if (!door) door = GetComponentInParent<DoorController>();
        door?.OnLatchStateChanged(IsLocked);
    }

    public List<Command> GetCommands()
    {
        var list = new List<Command>();
        list.Add(new Command { kind = IsLocked ? Command.Kind.Unlock : Command.Kind.Lock, label = IsLocked ? "Unlock" : "Lock" });
        return list;
    }

    public Command GetPrimaryCommand() => GetCommands()[0];

    public void Toggle() => ExecuteCommand(GetPrimaryCommand());

    public void ExecuteCommand(Command command)
    {
        if (command == null) return;

        if (command.kind == Command.Kind.Lock)
        {
            if (IsLocked) return;
            IsLocked = true;
            PlayClip(lockClip);
        }
        else if (command.kind == Command.Kind.Unlock)
        {
            if (!IsLocked) return;
            IsLocked = false;
            PlayClip(unlockClip);
        }
        else
        {
            return;
        }

        door?.OnLatchStateChanged(IsLocked);
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        if (!audioSource) audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
