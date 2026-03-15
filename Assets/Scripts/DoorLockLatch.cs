using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockLatch : MonoBehaviour, Interactable
{
    [SerializeField] bool startsLocked;
    [SerializeField] DoorController door;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lockClip;
    [SerializeField] AudioClip unlockClip;

    [SerializeField] float latchOffsetX  = 0.0176f;
    [SerializeField] float slideDuration  = 0.15f;

    public bool IsLocked { get; private set; }

    Vector3 lockedLocalPos;

    void Awake()
    {
        lockedLocalPos = transform.localPosition;
        IsLocked = startsLocked;
        if (!door) door = GetComponentInParent<DoorController>();
        door?.OnLatchStateChanged(IsLocked);
        ApplyLatchPosition();
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
        StopAllCoroutines();
        StartCoroutine(SlideToTarget());
    }

    IEnumerator SlideToTarget()
    {
        var target = lockedLocalPos;
        if (!IsLocked) target.x += latchOffsetX;

        var start = transform.localPosition;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            transform.localPosition = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        transform.localPosition = target;
    }

    void ApplyLatchPosition()
    {
        var pos = lockedLocalPos;
        if (!IsLocked) pos.x += latchOffsetX;
        transform.localPosition = pos;
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        if (!audioSource) audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
