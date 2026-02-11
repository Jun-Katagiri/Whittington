using UnityEngine;
using System.Collections.Generic;
using DunGen;

// Door class offers interaction capabilities for doors.
public class DoorController : MonoBehaviour, Interactable
{
    [SerializeField] List<Command> commands = new();
    [SerializeField] DoorLockLatch latch;
    [SerializeField] DoorLockBoltIndicator boltIndicator;


    [SerializeField] DoorLeaf doorLeaf;

    void Awake()
    {
        doorLeaf ??= GetComponentInChildren<DoorLeaf>();
        if (doorLeaf == null)
            Debug.LogError($"[DoorController] No DoorLeaf found on {name}");
        boltIndicator ??= GetComponentInChildren<DoorLockBoltIndicator>();
        if (boltIndicator == null)
            Debug.LogError($"[DoorController] No DoorLockBoltIndicator found on {name}");
        OnLatchStateChanged(latch != null && latch.IsLocked);
    }

    public void OnLatchStateChanged(bool isLocked)
    {
        // Update bolt indicator visibility
        if (boltIndicator != null)
        {
            boltIndicator.SetOccupied(isLocked);
        }
    }

    bool CanOpen() => latch == null || !latch.IsLocked;


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
                if (!CanOpen()) return;
                doorLeaf.Toggle();
                break;
            case Command.Kind.Lock:
                Debug.Log("Lock command should be sent to latch, not door");
                break;
            case Command.Kind.Unlock:
                Debug.Log("Unlock command should be sent to latch, not door");
                break;
            default:
                Debug.LogWarning($"[DoorController] ExecuteCommand called with unknown command kind on {name}");
                break;
        }
    }



}
