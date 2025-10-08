using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // 房间预制体
    public Room roomPrefab;
    // 需要生成的房间总数
    private int TargetRoomCount = 10;
    // 房间地图
    private RoomMap _roomMap;
    // 当前玩家所处房间节点
    private RoomNode _currentRoomNode;
    // 当前玩家所处房间
    public Room CurrentRoom => _currentRoomNode.Room;

    // 初始化（生成房间地图）
    public void Initialize()
    {
        // 如果生成失败则一直生成
        do
        {
            _roomMap = new RoomMap(this, TargetRoomCount, out _currentRoomNode);
        } while (!_roomMap.Generate());
    }

    // 根据预制体新建一个房间
    public Room NewRoom()
    {
        return Instantiate(roomPrefab, transform);
    }
    
    // 玩家移动到下一个房间时
    public void OnPlayerMoveToNextRoom(DirectionType dir)
    {
        _currentRoomNode = _currentRoomNode.GetChild(dir);
        if (!CurrentRoom.IsActivated)
            CurrentRoom.Activate();
    }
}
