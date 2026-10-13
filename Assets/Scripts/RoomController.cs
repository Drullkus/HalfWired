using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{
    [SerializeField] private double wallVanishChance = 0.25;
    
    [SerializeField] private long index;
    [SerializeField] private float height;

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

        transform.position = new Vector3(transform.position.x, index * height, transform.position.z);
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
    }

    public long GetIndex()
    {
        return index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        tower.PlayerEnteredRoom(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        tower.PlayerExitedRoom(this);
    }

}
