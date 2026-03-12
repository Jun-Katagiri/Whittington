using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDryerController : MonoBehaviour, Interactable
{
    [Header("Interaction")]
    [SerializeField] string labelIdle    = "Use";
    [SerializeField] string labelRunning = "Running...";
    [SerializeField] float  duration     = 30f;

    [Header("Feedback")]
    [SerializeField] AudioSource audioSource;

    bool isRunning = false;

    readonly List<Command> commands = new();
    Command primaryCommand;

    void Awake() => EnsureCommand();

    public List<Command> GetCommands()
    {
        EnsureCommand();
        return commands;
    }

    public Command GetPrimaryCommand()
    {
        EnsureCommand();
        return primaryCommand;
    }

    public void ExecuteCommand(Command command)
    {
        if (command == null || command.kind != Command.Kind.Toggle)
            return;

        if (isRunning)
            return;

        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        isRunning = true;
        UpdateLabel();

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }

        yield return new WaitForSeconds(duration);

        Stop();
    }

    void Stop()
    {
        isRunning = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }

        UpdateLabel();
    }

    void UpdateLabel()
    {
        if (primaryCommand != null)
            primaryCommand.label = isRunning ? labelRunning : labelIdle;
    }

    void EnsureCommand()
    {
        if (primaryCommand == null)
            primaryCommand = new Command { kind = Command.Kind.Toggle, label = labelIdle };

        if (commands.Count == 0)
            commands.Add(primaryCommand);
        else
            commands[0] = primaryCommand;
    }
}
