using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private double wallVanishChance = 0.25;
    
    [SerializeField] private long index;

    [SerializeField] private GameObject wallPX;
    [SerializeField] private GameObject wallNX;
    [SerializeField] private GameObject wallPZ;
    [SerializeField] private GameObject wallNZ;

    [SerializeField] private TowerController tower;

    public void InitializeRoom(TowerController towerIn, long indexIn)
    {
        tower = towerIn;
        index = indexIn;
        
        Random.InitState(index.GetHashCode());
        Random.InitState(Random.value.GetHashCode());
        
        wallPX.SetActive(Random.value >= wallVanishChance);
        wallNX.SetActive(Random.value >= wallVanishChance);
        wallPZ.SetActive(Random.value >= wallVanishChance);
        wallNZ.SetActive(Random.value >= wallVanishChance);
    }

}
