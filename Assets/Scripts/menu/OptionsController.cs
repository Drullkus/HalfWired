using UnityEngine;

public class OptionsController : MonoBehaviour
{

    public void UpdateTowerPadding(float value)
    {
        Debug.Log($"Tower Padding set to {value}");
        GameMaster.Instance.TowerPadding = Mathf.RoundToInt(value);
    }

}
