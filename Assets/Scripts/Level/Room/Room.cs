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
    // 房间样式
    private RoomLayout _layout;
    
    // 玩家是否进入过该房间
    public bool IsActivated { get; set; }
    
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
    
    // 房间所有的可用门列表（就是显示出来的门）
    private List<Door>  _doors = new();
    
    // 随机数生成器
    private readonly System.Random _rand = new();

    private void Awake()
    {
        Initialize();
    }

    // 初始化门
    private void Initialize()
    {
        IsActivated = false;
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

    // 玩家第一次到的时候激活房间
    public void Activate()
    {
        IsActivated = true;
        if (_layout.enemyList.Count != 0)
        {
            foreach (var pair in _layout.enemyList)
            {
                var enemy = Instantiate(pair.gameObject, transform);
                enemy.transform.position = transform.position;
            }
            CloseDoor();
        }
    }
    
    // 初始化房间样式
    public void InitializeLayout()
    {
        // 获取相同类型的备选房间列表
        var layouts = _roomTypeDict[type];
        // 随机选一个房间样式
        _layout = layouts[_rand.Next(layouts.Count)];
        foreach (var floor in floorCache)
        {
            floor.SetLayout(_layout.floor);
        }
        // 对于初始房间，还需要初始化 tip
        if (type == RoomType.Start)
            tip.sprite = _layout.tip;
    }
    
    // 激活特定方向的门
    public void ActivateDoor(DirectionType dir)
    {
        var door = _doorDict[dir];
        door.gameObject.SetActive(true);
        // 取消对应方向的门墙
        _doorWallDict[dir].gameObject.SetActive(false);
        _doors.Add(door);
    }
    
    // 设定某个朝向的门为某样式
    public void SetDoorStyle(DirectionType dir, RoomType roomType)
    {
        _doorDict[dir].SetStyle(roomType);
    }
    
    // 关闭房间所有的门（播放动画）
    public void CloseDoor() => _doors.ForEach(door => door.Close());
    
    // 打开房间所有的门（播放动画）
    public void OpenDoor() => _doors.ForEach(door => door.Open());
}
