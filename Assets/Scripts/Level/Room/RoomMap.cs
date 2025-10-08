using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/**
 * 房间地图
 */
public class RoomMap
{
    // 挂载的关卡实例
    private Level _level;
    // 需要创建的房间数量
    private readonly int _targetRoomCount;
    
    // 储存所有房间节点的哈希字典
    private readonly Dictionary<(int, int), RoomNode> _map =  new();
    // 储存叶子结点的哈希集合
    private readonly HashSet<RoomNode> _leaves = new();
    // 待初始化的房间集合
    private readonly HashSet<Room> _pendingRooms = new();
    // 随机数生成器
    private readonly System.Random _rand = new();
    
    public RoomMap(Level level, int targetRoomCount, out RoomNode currentRoomNode)
    {
        _level = level;
        _targetRoomCount = targetRoomCount;
        // 创建起始房间节点
        var startRoomNode = new RoomNode(_level.NewRoom());
        startRoomNode.Room.type = RoomType.Start;
        currentRoomNode = startRoomNode;
        _pendingRooms.Add(startRoomNode.Room);
        // 房间节点和坐标双向绑定
        _map.Add((0, 0), startRoomNode);
        startRoomNode.Location = (0, 0);
        // 将起始房间加入叶子结点集合
        _leaves.Add(startRoomNode);
    }

    // 生成房间地图
    public bool Generate()
    {
        while (_map.Count < _targetRoomCount && _leaves.Count > 0)
        {
            // —— 1. 随机选取叶子结点与该节点的可延伸方向 ——
            // 随机选取一个叶子结点
            var parentNode = _leaves.ToList()[_rand.Next(_leaves.Count)];
            // 随机选取该叶子结点的一个可用方向
            var dir = parentNode.LeafDirs[_rand.Next(parentNode.LeafCount)];
            
            // —— 2. 判断该方向是否可以创建新房间 ——
            var (x, y) = parentNode.Location;
            var (dx, dy) = DirectionUtil.Offset[dir];
            // 如果该坐标已经有房间，则重新选取
            if (_map.ContainsKey((x + dx, y + dy))) 
                continue;
            
            // —— 3. 创建新房间并移动到相应位置 ——
            var childRoom = _level.NewRoom();
            var parentPosition = parentNode.Room.gameObject.transform.position;
            // 子房间应该移动的位移
            var diff = new Vector3(dx * Room.Length, dy * Room.Width, 0);
            // 移动新房间到正确位置
            childRoom.gameObject.transform.position = parentPosition + diff;
            
            // —— 4. 创建新结点并建立联系 ——
            var childNode = new RoomNode(childRoom);
            parentNode.LinkTo(childNode, dir);
            // 如果父节点已经不是叶子结点了，则移出集合
            if (!parentNode.IsLeaf)
                _leaves.Remove(parentNode);
            // 向地图与叶子结点集合中添加新节点
            _map.Add((x + dx, y + dy), childNode);
            _leaves.Add(childNode);
            _pendingRooms.Add(childRoom);
        }
        
        // —— 5. 安置 BOSS 房或宝藏房 ——
        // 计算只有一个门的房间（用于改造为 BOSS 房或宝藏房），并按照距离升序排序
        var oneDoorRoomList = _leaves
            .Where(node => node.Room.type == RoomType.Normal && node.DoorCount == 1)
            .OrderBy(node => node.Distance)
            .ToList();
        // 如果数量少于 2，则本次生成失败，需要重新生成
        if (oneDoorRoomList.Count < 2)
            return false;
        
        // 最远的一间房为 BOSS 房
        SetRoomType(oneDoorRoomList.Last(), RoomType.Boss);
        // 最近的一间房为宝藏房
        SetRoomType(oneDoorRoomList[0], RoomType.Treasure);
        // 如果还有多的房间则也设定成宝藏房
        if (oneDoorRoomList.Count > 2)
            SetRoomType(oneDoorRoomList[1], RoomType.Treasure);
        
        // 初始化房间
        foreach (var room in _pendingRooms)
        {
            room.InitializeLayout();
        }
        
        return true;
    }

    // 设置门的样式（只用于 BOSS 房和宝藏房）
    private void SetRoomType(RoomNode node, RoomType type)
    {
        // 门的朝向
        var dir = node.FirstDirection;
        node.Room.type = type;
        node.Room.SetDoorStyle(dir, type);
        // 还要设置相反方向的门样式
        node.GetChild(dir).Room.SetDoorStyle(DirectionUtil.GetOpposite(dir), type);
    }
}

/**
 * 房间节点
 * 仿照链表设计，实际上就是个四向链表
 */
public class RoomNode
{
    // 四个方向的子节点
    private RoomNode _up;
    private RoomNode _down;
    private RoomNode _left;
    private RoomNode _right;
    
    // 房间的实例
    public Room Room { get; }
    // 房间的坐标
    public (int, int) Location { get; set; }
    
    public RoomNode(Room room)
    {
        Room = room;
    }
    
    // 根据方向获取子节点
    public RoomNode GetChild(DirectionType direction) => direction switch
    {
        DirectionType.Up => _up,
        DirectionType.Down => _down,
        DirectionType.Left => _left,
        DirectionType.Right => _right,
        _ => null
    };
    
    // 门的数量（就是非空子节点数）
    public int DoorCount =>
        (_up    == null ? 0 : 1) +
        (_down  == null ? 0 : 1) +
        (_left  == null ? 0 : 1) +
        (_right == null ? 0 : 1);
    // 叶子数量（就是可选的延伸方向）
    public int LeafCount => 4 - DoorCount;
    // 该节点是否是叶子结点
    public bool IsLeaf => LeafCount != 0;
    // 该节点所有的叶子方向
    public DirectionType[] LeafDirs => DirectionUtil.All.Where(d => GetChild(d) == null).ToArray();
    // 该节点的绝对值距离
    public int Distance => Math.Abs(Location.Item1) + Math.Abs(Location.Item2);
    // 返回第一个非空子节点的方向（其实就是用来找单节点房间的节点朝向的）
    public DirectionType FirstDirection => 
        DirectionUtil.All.FirstOrDefault(dir => GetChild(dir) != null);
    
    
    // 与另一个房间结点相连
    public void LinkTo(RoomNode node, DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Up:
                _up = node;
                node._down = this;
                node.Location = (Location.Item1, Location.Item2 + 1);
                break;
            case DirectionType.Down:
                _down = node;
                node._up = this;
                node.Location = (Location.Item1, Location.Item2 - 1);
                break;
            case DirectionType.Left:
                _left = node;
                node._right = this;
                node.Location = (Location.Item1 - 1, Location.Item2);
                break;
            case DirectionType.Right:
                _right = node;
                node._left = this;
                node.Location = (Location.Item1 + 1, Location.Item2);
                break;
            default:
                break;
        }
        // 激活本节点当前方向的门
        Room.ActivateDoor(direction);
        // 激活子节点相反方向的门
        node.Room.ActivateDoor(DirectionUtil.GetOpposite(direction));
    }
}