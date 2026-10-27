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

    [SerializeField] private bool regenerate; // Toybox trick
    
    private readonly Dictionary<Int64, GameObject> _rooms = new();
    private readonly HashSet<RoomController> _playerOverlapping = new();
    private int salt;
    private int roomHeight = 12;

    public void OnValidate()
    {
        if (regenerate)
        {
            regenerate = false;
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerMidY = player.transform.position.y + player.GetComponent<CharacterController>().height * 0.5f;
            var roomY = WorldSpaceYToRoomIndex(playerMidY);
            // Debug.Log($"Regenerating rooms centered on {roomY}");
            RebuildTower(roomY, true);
        }
    }

    private int WorldSpaceYToRoomIndex(float worldY)
    {
        return Mathf.RoundToInt(worldY / roomHeight);
    }

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

    public void RebuildTower(long playerRoom, bool all = false)
    {
        // Debug.Log($"Rebuilding tower from room {playerRoom}");
        
        DestroyFarRooms(playerRoom, all);
        InitializeMissingRooms(playerRoom);
        
        var towerEndPadding = GameMaster.Instance.TowerPadding + 1;
        towerBottom.position = new Vector3(0, (playerRoom - towerEndPadding) * roomHeight, 0);
        towerTop.position = new Vector3(0, (playerRoom + towerEndPadding) * roomHeight, 0);
    }

    private void DestroyFarRooms(long playerRoom, bool all = false)
    {
        var farRooms = all ? _rooms.Keys.ToList() : _rooms.Keys
            .Where(key => Math.Abs(key - playerRoom) > GameMaster.Instance.TowerPadding)
            .ToList();

        foreach (var farRoomIndex in farRooms)
        {
            Destroy(_rooms[farRoomIndex]);
            _rooms.Remove(farRoomIndex);
            // Debug.Log($"Removed {farRoomIndex}");
        }
    }

    private void InitializeMissingRooms(long playerRoom)
    {
        var towerPadding = GameMaster.Instance.TowerPadding;
        for (var roomIndex = playerRoom - towerPadding; roomIndex <= playerRoom + GameMaster.Instance.TowerPadding; roomIndex++)
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
