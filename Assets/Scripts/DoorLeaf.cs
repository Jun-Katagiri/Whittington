using UnityEngine;

public class DoorLeaf : MonoBehaviour
{
    [SerializeField] bool isOpen;

    public virtual void Open()
    {
        if (isOpen)
            return;

        isOpen = true;
        PlayOpenSound();
    }

    public virtual void Close()
    {
        if (!isOpen)
            return;

        isOpen = false;
        PlayCloseSound();
    }

    public virtual void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public virtual bool IsOpen => isOpen;
    public virtual void PlayFailedToggleSound() => PlayFailedOpenSound();

    // Todo: play sound effects when opening/closing, and when locked/unlocked.
    // Also, when locked, play a different sound when trying to open.
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openClip;
    [SerializeField] AudioClip closeClip;
    [SerializeField] AudioClip lockedClip;
    [SerializeField] AudioClip unlockedClip;
    [SerializeField] AudioClip failedOpenClip;

    protected AudioSource GetOrCreateAudioSource()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        return audioSource;
    }

    protected void SetAudioSource(AudioSource source) => audioSource = source;

    protected void SetSoundClips(
        AudioClip open,
        AudioClip close,
        AudioClip locked,
        AudioClip unlocked,
        AudioClip failedOpen)
    {
        openClip = open;
        closeClip = close;
        lockedClip = locked;
        unlockedClip = unlocked;
        failedOpenClip = failedOpen;
    }

    protected bool PlayOpenSound() => PlayClip(openClip);
    protected bool PlayCloseSound() => PlayClip(closeClip);
    protected bool PlayLockedSound() => PlayClip(lockedClip);
    protected bool PlayUnlockedSound() => PlayClip(unlockedClip);
    protected bool PlayFailedOpenSound() => PlayClip(failedOpenClip);

    protected bool PlayClip(AudioClip clip)
    {
        if (clip == null)
            return false;

        GetOrCreateAudioSource().PlayOneShot(clip);
        return true;
    }
}

