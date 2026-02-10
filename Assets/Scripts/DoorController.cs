using UnityEngine;
using System.Collections.Generic;

// Door class offers interaction capabilities for doors.
public class DoorController : MonoBehaviour, Interactable
{
    [SerializeField] List<Command> commands = new();
    [SerializeField] List<Lock> locks = new();

    [SerializeField] DoorLeaf doorLeaf;

    void Awake()
    {
        doorLeaf ??= GetComponentInChildren<DoorLeaf>();
        if (doorLeaf == null)
            Debug.LogError($"[DoorController] No DoorLeaf found on {name}");
    }


    public List<Command> GetCommands()
    {
        return commands;
    }

    public Command GetPrimaryCommand()
    {

        if (commands.Count > 0)
            return commands[0];
        return null;

    }


    public void Toggle()
    {
        if (doorLeaf == null)
        {
            Debug.LogError($"[DoorController] Toggle called with no DoorLeaf on {name}");
            return;
        }

        if (GetPrimaryCommand() != null)
        {
            doorLeaf.Toggle();
        }
        else
        {
            Debug.LogWarning($"[DoorController] Toggle called but no primary command on {name}");
        }
    }

    public void ExecuteCommand(Command command)
    {
        if (doorLeaf == null)
        {
            Debug.LogError($"[DoorController] ExecuteCommand called with no DoorLeaf on {name}");
            return;
        }

        if (command == null)
        {
            Debug.LogWarning($"[DoorController] ExecuteCommand called with null command on {name}");
            return;
        }

        switch (command.kind)
        {
            case Command.Kind.Open:
                doorLeaf.Open();
                break;
            case Command.Kind.Close:
                doorLeaf.Close();
                break;
            case Command.Kind.Toggle:
                doorLeaf.Toggle();
                break;
            case Command.Kind.Lock:
                foreach (var l in locks)
                    l.LockAction();
                break;
            case Command.Kind.Unlock:
                foreach (var l in locks)
                    l.UnlockAction();
                break;
            default:
                Debug.LogWarning($"[DoorController] ExecuteCommand called with unknown command kind on {name}");
                break;
        }
    }



}
