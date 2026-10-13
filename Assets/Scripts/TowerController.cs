using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    
    [SerializeField] private GameObject roomPrefab;
    
    private readonly Dictionary<Int64, GameObject> _rooms = new();
    private readonly HashSet<RoomController> _playerOverlapping = new();

    public void Start()
    {
        InitializeMissingRooms(0);
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

    private void RebuildTower(long playerRoom)
    {
        // Debug.Log($"Rebuilding tower from room {playerRoom}");
        
        DestroyFarRooms(playerRoom);
        InitializeMissingRooms(playerRoom);
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

            var newRoom = Instantiate(roomPrefab, transform);
            newRoom.GetComponent<RoomController>().InitializeRoom(this, roomIndex);
            _rooms.Add(roomIndex, newRoom);
        }
    }

}
