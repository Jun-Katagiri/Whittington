using UnityEngine;

public class XformDump : MonoBehaviour
{
    public Transform container;
    public Transform room;
    public Transform playerRoot; // CharacterController‚ª•t‚¢‚Ä‚éŠK‘w

    void Start()
    {
        Dump("CONTAINER", container);
        Dump("ROOM", room);
        Dump("PLAYER", playerRoot);
    }

    void Dump(string label, Transform t)
    {
        if (!t) { Debug.Log(label + ": (null)"); return; }

        Debug.Log(
            $"{label}\n" +
            $"  name={t.name}\n" +
            $"  world pos={t.position} rot={t.rotation.eulerAngles} scale={t.lossyScale}\n" +
            $"  local pos={t.localPosition} rot={t.localRotation.eulerAngles} scale={t.localScale}\n" +
            $"  parent={(t.parent ? t.parent.name : "(none)")}"
        );
    }
}
