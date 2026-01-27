using UnityEngine;
using DunGen;

public class RuntimeDungeonTest : MonoBehaviour
{
    public RuntimeDungeon dungeon;

    void Start()
    {
        dungeon.Generate();
    }
}
