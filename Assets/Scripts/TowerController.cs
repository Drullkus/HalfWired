using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerController : MonoBehaviour
{
    
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform towerTop;
    [SerializeField] private Transform towerBottom;
    
    private readonly Dictionary<Int64, GameObject> _rooms = new();
    private readonly HashSet<RoomController> _playerOverlapping = new();
    private int salt;
    private int roomHeight = 12;

    public void Start()
    {
        InitializeMissingRooms(0);
        salt = Random.Range(0, int.MaxValue);
    }

    public void PlayerEnteredRoom(RoomController room)
    {
        _playerOverlapping.Add(room);
        
        // Debug.Log($"PlayerEnteredRoom: _playerOverlapping.Count = {_playerOverlapping.Count}, room indices [{string.Join(", ", _playerOverlapping.Select(room1 => room1.GetIndex()))}]");
        
        if (_playerOverlapping.Count == 1)
        {
            RebuildTower(_playerOverlapping.First().GetIndex());
        }
    }

    public void PlayerExitedRoom(RoomController room)
    {
        _playerOverlapping.Remove(room);
        
        // Debug.Log($"PlayerExitedRoom: _playerOverlapping.Count = {_playerOverlapping.Count}, room indices [{string.Join(", ", _playerOverlapping.Select(room1 => room1.GetIndex()))}]");

        if (_playerOverlapping.Count == 1)
        {
            RebuildTower(_playerOverlapping.First().GetIndex());
        }
    }

    public void RebuildTower(long playerRoom)
    {
        // Debug.Log($"Rebuilding tower from room {playerRoom}");
        
        DestroyFarRooms(playerRoom);
        InitializeMissingRooms(playerRoom);
        towerBottom.position = new Vector3(0, (playerRoom - 3) * roomHeight, 0);
        towerTop.position = new Vector3(0, (playerRoom + 3) * roomHeight, 0);
    }

    private void DestroyFarRooms(long playerRoom)
    {
        var farRooms = _rooms.Keys
            .Where(key => Math.Abs(key - playerRoom) > 2)
            .ToList();

        foreach (var farRoomIndex in farRooms)
        {
            Destroy(_rooms[farRoomIndex]);
            _rooms.Remove(farRoomIndex);
        }
    }

    private void InitializeMissingRooms(long playerRoom)
    {
        for (var roomIndex = playerRoom - 2; roomIndex <= playerRoom + 2; roomIndex++)
        {
            if (_rooms.ContainsKey(roomIndex)) continue;

            Random.InitState(roomIndex.GetHashCode() ^ salt);
            Random.InitState(Random.value.GetHashCode());

            var position = new Vector3(0, roomIndex * 12, 0);
            var rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            
            var newRoom = Instantiate(roomPrefab, position, rotation, transform);
            newRoom.GetComponent<RoomController>().InitializeRoom(this, roomIndex);
            _rooms.Add(roomIndex, newRoom);
        }
    }

}
