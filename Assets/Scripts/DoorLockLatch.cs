using UnityEngine;
using System.Collections.Generic;

public class DoorLockLatch : MonoBehaviour, Interactable
{
    [SerializeField] bool startsLocked;
    [SerializeField] DoorController door; // 親ドア参照（任意：自動取得でもOK）

    public bool IsLocked { get; private set; }

    void Awake()
    {
        IsLocked = startsLocked;
        if (!door) door = GetComponentInParent<DoorController>();
        door?.OnLatchStateChanged(IsLocked);
    }

    public List<Command> GetCommands()
    {
        // ここは「内側で触れた時」しか来ない前提なので、計算不要
        // 状態で出し分け
        var list = new List<Command>();
        list.Add(new Command { kind = IsLocked ? Command.Kind.Unlock : Command.Kind.Lock, label = IsLocked ? "Unlock" : "Lock" });
        return list;
    }

    public Command GetPrimaryCommand() => GetCommands()[0];

    public void Toggle() => ExecuteCommand(GetPrimaryCommand());

    public void ExecuteCommand(Command command)
    {
        if (command == null) return;

        if (command.kind == Command.Kind.Lock) IsLocked = true;
        else if (command.kind == Command.Kind.Unlock) IsLocked = false;
        else return;

        door?.OnLatchStateChanged(IsLocked);
    }
}
