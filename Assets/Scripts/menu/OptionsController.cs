using UnityEngine;

public class OptionsController : MonoBehaviour
{

    public void UpdateTowerPadding(float value)
    {
        Debug.Log($"Tower Padding set to {value}");
        GameMaster.Instance.TowerPadding = Mathf.RoundToInt(value);
    }

    public void UpdateRocketDifficulty(float value)
    {
        Debug.Log($"Rocket Difficulty set to {value}");
        GameMaster.Instance.RocketDifficulty = value;
    }

}
