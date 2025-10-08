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

    private void Awake()
    {
        GenerateRoomMap();
    }

    // 根据预制体新建一个房间
    public Room NewRoom()
    {
        return Instantiate(roomPrefab, transform);
    }
    
    // 生成房间地图
    private void GenerateRoomMap()
    {
        // 如果生成失败则一直生成
        do
        {
            _roomMap = new RoomMap(this, TargetRoomCount);
        } while (!_roomMap.Generate());
    }
}
