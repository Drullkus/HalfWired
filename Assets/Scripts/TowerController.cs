using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Dictionary<Int64, GameObject> rooms;
    
    public void Awake()
    {
        foreach (var indexedRoom in rooms)
        {
            indexedRoom.Value.GetComponent<RoomController>().InitializeRoom(this, indexedRoom.Key);
        }
    }

}
