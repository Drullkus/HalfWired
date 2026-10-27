using Unity.Scripting.LifecycleManagement;
using UnityEngine;

[AutoStaticsCleanup]
public class GameMaster
{
    public static GameMaster Instance { get; } = new();

    public int TowerPadding = 2;
    
    public bool MenuOpen = true;

    private GameMaster()
    {
        Debug.Log("GameMaster initialized");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
