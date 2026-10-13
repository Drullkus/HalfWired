using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Dictionary<Int64, GameObject> rooms;
    
    public void Awake()
    {
        foreach (var keyValuePair in rooms)
        {
            keyValuePair.Value.GetComponent<RoomController>().InitializeRoom(this, keyValuePair.Key);
        }
    }
}
