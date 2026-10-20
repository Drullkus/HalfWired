using System.Collections.Generic;
using System.Linq;
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

    [DictionaryDisplay(keyLabel = "Prefab", valueLabel = "Weight")]
    [SerializeField] private Dictionary<GameObject, int> doodadOptions = new();

    [SerializeField] private int objectCountLowerBound = 2;
    [SerializeField] private int objectCountUpperBound = 4;
    [SerializeField] private GameObject doodadsVolume;

    [SerializeField] private TowerController tower;

    public void InitializeRoom(TowerController towerIn, long indexIn)
    {
        // Unity.Random is seeded outside of this call in the Tower Controller

        tower = towerIn;
        index = indexIn;
        
        PlaceDoodads(indexIn);
        
        wallPX.SetActive(Random.value >= wallVanishChance);
        wallNX.SetActive(Random.value >= wallVanishChance);
        wallPZ.SetActive(Random.value >= wallVanishChance);
        wallNZ.SetActive(Random.value >= wallVanishChance);
    }

    private void PlaceDoodads(long indexIn)
    {
        if (doodadOptions.Count <= 0) return;
        int objectSpawns = Random.Range(objectCountLowerBound, objectCountUpperBound + 1);
        if (objectSpawns <= 0) return;
        int totalWeight = doodadOptions.Values.Aggregate((a, b) => a + b);
        if (totalWeight <= 0) return;

        var doodadsBox = doodadsVolume.GetComponent<BoxCollider>();
        var placeRadiusX = doodadsBox.size.x * 0.5f;
        var placeRadiusY = doodadsBox.size.y * 0.5f;
        var placeRadiusZ = doodadsBox.size.z * 0.5f;
        var roomHeight = 12;
        
        for (int objectIndex = 0; objectIndex < objectSpawns; objectIndex++)
        {
            var chosenPrefab = WeightedChoose(totalWeight, doodadOptions);
            if (chosenPrefab == null) continue;

            float xPos = Mathf.Lerp(-placeRadiusX, placeRadiusX, Random.value);
            float zPos = Mathf.Lerp(-placeRadiusZ, placeRadiusZ, Random.value);
            float yPos = indexIn * roomHeight - placeRadiusY - 0.5f;
            Instantiate(chosenPrefab, new Vector3(xPos, yPos, zPos), new Quaternion(0, 1, 0, Random.value), doodadsVolume.transform);
        }
    }

    private static V WeightedChoose<V>(int totalWeight, Dictionary<V, int> dictionary)
    {
        // Process dictionary like a weighted list
        var chosen = Random.Range(0, totalWeight);
        foreach (var (obj, weight) in dictionary)
        {
            if (chosen < weight) return obj;
            chosen -= weight;
        }
        return default;
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
