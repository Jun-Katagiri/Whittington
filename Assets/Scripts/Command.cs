[System.Serializable]
public class Command
{
    public Kind kind;
    public string label;
    public enum Kind { Open, Close, Toggle, Lock, Unlock }
}
