using Unity.Scripting.LifecycleManagement;

[AutoStaticsCleanup]
public class GameMaster
{
    public static GameMaster Instance { get; } = new();

    public int TowerPadding = 2;

    private GameMaster()
    {
    }
    
}
