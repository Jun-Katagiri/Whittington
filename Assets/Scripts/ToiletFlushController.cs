using System.Collections.Generic;
using UnityEngine;

public class ToiletFlushController : MonoBehaviour, Interactable
{
    [SerializeField] AudioSource soundEffect;
    [SerializeField] Animator animatorObject;
    [SerializeField] string waterAnimationName = "Toilet Water Flush";
    [SerializeField] string commandLabel = "Flush";
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.1f;
    [SerializeField] float cooldownDuration = 5f;

    float lastInteractionTime = -999f;

    readonly List<Command> commands = new();
    Command primaryCommand;

    void Awake()
    {
        if (cooldownDuration <= 0f)
            cooldownDuration = 5f;
        EnsureCommand();
    }

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
        if (command == null)
            return;

        if (command.kind != Command.Kind.Toggle)
            return;

        if (Time.time < lastInteractionTime + cooldownDuration)
            return;

        Flush();
        lastInteractionTime = Time.time;
    }

    void Flush()
    {
        bool playedAnyAnimation = false;

        // Water animation may live on a different Animator in the same prefab hierarchy.
        var root = animatorObject != null ? animatorObject.transform.root : transform.root;
        var animators = root.GetComponentsInChildren<Animator>(true);
        foreach (var animator in animators)
        {
            if (animator == null)
                continue;

            playedAnyAnimation |= PlayStateIfExists(animator, waterAnimationName);
        }

        if (!playedAnyAnimation && animatorObject != null)
            playedAnyAnimation |= PlayStateIfExists(animatorObject, waterAnimationName);

        if (soundEffect != null)
        {
            soundEffect.pitch = Random.Range(minPitch, maxPitch);
            soundEffect.Play();
        }
    }

    bool PlayStateIfExists(Animator animator, string stateName)
    {
        if (animator == null || string.IsNullOrEmpty(stateName))
            return false;

        int shortNameHash = Animator.StringToHash(stateName);
        int fullPathHash = Animator.StringToHash($"Base Layer.{stateName}");

        bool hasShort = animator.HasState(0, shortNameHash);
        bool hasFull = animator.HasState(0, fullPathHash);
        if (!hasShort && !hasFull)
            return false;

        if (!animator.enabled)
            animator.enabled = true;

        animator.Play(hasFull ? $"Base Layer.{stateName}" : stateName, 0, 0f);
        return true;
    }

    void EnsureCommand()
    {
        if (primaryCommand == null)
            primaryCommand = new Command { kind = Command.Kind.Toggle, label = commandLabel };
        else
            primaryCommand.label = commandLabel;

        if (commands.Count == 0)
            commands.Add(primaryCommand);
        else
            commands[0] = primaryCommand;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // Warns you in the console if you assigned an AudioSource from another stall
        if (soundEffect != null && !soundEffect.transform.IsChildOf(transform))
        {
            Debug.LogWarning($"[ToiletFlushController] The assigned soundEffect on '{name}' belongs to a different hierarchy. Did you assign the wrong 'Toilet Sound'?", this);
        }
    }

    void Reset()
    {
        // Auto-assigns the references so you don't have to drag and drop them manually
        if (soundEffect == null)
            soundEffect = GetComponentInChildren<AudioSource>();

        if (animatorObject == null)
            animatorObject = GetComponentInChildren<Animator>();
    }
#endif
}
