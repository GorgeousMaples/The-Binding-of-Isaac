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
    
    // 单位坐标长度（以房间中心为原点，长进行 8 等分，宽进行 6 等分得到的长度）
    private static readonly float UnitLength = 5.42f / 8;
    private static readonly float UnitWidth = 3.05f / 8;
    
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
    
    // 已击杀敌人数
    private int _enemyKilledCount = 0;
    
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
        // 初始化障碍物
        if (_layout.obstacleList.Count != 0)
        {
            foreach (var pair in _layout.obstacleList)
            {
                var obstacle = Instantiate(pair.gameObject, transform);
                SetPosition(obstacle.transform, pair.position);
            }
        }
    }

    // 玩家第一次到的时候激活房间
    public void Activate()
    {
        IsActivated = true;
        // 初始化怪物
        if (_layout.enemyList.Count != 0)
        {
            foreach (var pair in _layout.enemyList)
            {
                var enemy = GameManager.Instance.InstantiateEnemy(pair.gameObject);
                enemy.Killed += OnEnemyKilled;
                SetPosition(enemy.transform, pair.position);
            }
            CloseDoor();
        }
    }
    
    // 击杀敌人钩子
    private void OnEnemyKilled()
    {
        _enemyKilledCount++;
        // 代表已经清除这个房间里所有的怪
        if (_enemyKilledCount == _layout.enemyList.Count)
        {
            OnClearRoom();
        }
    }

    // 将生成的物体移到正确的位置上
    private void SetPosition(Transform obj, Vector2Int pos)
    {
        var distance = new Vector3(UnitLength * pos.x, UnitWidth * pos.y, 0);
        obj.position = transform.position + distance;
    }
    
    // 当玩家清除房间后
    private void OnClearRoom()
    {
        OpenDoor();
        // 未来还会有其他逻辑
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
