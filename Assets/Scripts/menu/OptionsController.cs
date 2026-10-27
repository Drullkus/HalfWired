using UnityEngine;

public class OptionsController : MonoBehaviour
{

    public void UpdateTowerPadding(int value)
    {
        Debug.Log($"Tower Padding set to {value}");
        GameMaster.Instance.TowerPadding = value;
    }

}
