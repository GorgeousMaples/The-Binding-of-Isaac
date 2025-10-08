using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 房间类型
public enum RoomType
{
    Normal = 0, Start, Boss, Treasure, Shop
}

public class Room : MonoBehaviour
{
    // 长度
    public static readonly float Length = 7.55f;
    // 宽度
    public static readonly float Width = 4.68f;
    
    [HideInInspector]
    // 房间类型
    public RoomType type;
    
    // 缓存
    [SerializeField] private Door[]  doorCache;
    [SerializeField] private Wall[]  doorWallCache;
    [SerializeField] private Floor[] floorCache;
    [SerializeField] private SpriteRenderer tip;
    
    // 四个方向的门的字典
    private readonly Dictionary<DirectionType, Door> _doorDict = new();
    // 四个方向的门墙的字典
    private readonly Dictionary<DirectionType, Wall> _doorWallDict = new();
    // 房间样式的字典
    private Dictionary<RoomType, List<RoomLayout>> _roomTypeDict;
    
    // 随机数生成器
    private readonly System.Random _rand = new();

    private void Awake()
    {
        Initialize();
    }

    // 初始化门
    private void Initialize()
    {
        // 将四个门添加到字典中，并先都不激活
        foreach (var door in doorCache)
        {
            _doorDict.Add(door.direction, door);
            door.gameObject.SetActive(false);
        }
        
        // 将门墙添加到字典中
        foreach (var wall in doorWallCache)
        {
            if (wall.isDoorWall)
                _doorWallDict.Add(wall.direction, wall);
        }
        
        var allLayouts = new List<RoomLayout>(Resources.LoadAll<RoomLayout>("RoomLayout"));
        _roomTypeDict = allLayouts
            .GroupBy(r => r.type)
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );
    }
    
    // 使用房间样式
    public void UseLayout()
    {
        // 获取相同类型的备选房间列表
        var layouts = _roomTypeDict[type];
        // 随机选一个房间样式
        var layout = layouts[_rand.Next(layouts.Count)];
        foreach (var floor in floorCache)
        {
            floor.SetLayout(layout.floor);
        }
        // 对于初始房间，还需要初始化 tip
        if (type == RoomType.Start)
            tip.sprite = layout.tip;
    }
    
    // 激活特定方向的门
    public void ActivateDoor(DirectionType dir)
    {
        _doorDict[dir].gameObject.SetActive(true);
        // 取消对应方向的门墙
        _doorWallDict[dir].gameObject.SetActive(false);
    }
    
    // 设定某个朝向的门为某样式
    public void SetDoorStyle(DirectionType dir, RoomType roomType)
    {
        _doorDict[dir].SetStyle(roomType);
    }
}
