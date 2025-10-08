using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Door : Obstacle
{
    // 朝向
    public DirectionType direction;
    
    // 玩家经过该门时的移动向量
    private Vector3 PlayerMoveVector =>  direction switch
    {
        DirectionType.Up => Vector2.up,
        DirectionType.Down => Vector2.down,
        DirectionType.Left => Vector2.left,
        DirectionType.Right => Vector2.right,
        _ => Vector2.zero
    };
    
    // 框架样式
    [SerializeField] private SpriteRenderer frame;
    // 门洞样式
    [SerializeField] private SpriteRenderer hole;
    // 门扇样式
    [SerializeField] private SpriteRenderer leftLeaf;
    [SerializeField] private SpriteRenderer rightLeaf;
    
    // 初始状态
    private bool _isOpen = true;
    // 体积碰撞
    private Collider2D _collider;
    // 动画器
    private Animator _animator;
    
    // 玩家实例
    private Player Player => GameManager.Instance.player;
    // 主相机
    private Camera Camera => GameManager.Instance.mainCamera;
    
    // 房间样式的字典
    private readonly Dictionary<RoomType, DoorStyle> _styleDict = new();
    
    // 不可破坏
    public bool IsDestructible => false;
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        Change(_isOpen);
        
        // 导入样式
        var allStyles = new List<DoorStyle>(Resources.LoadAll<DoorStyle>("DoorStyle"));
        allStyles.ForEach(style => _styleDict[style.type] = style);
    }

    // 改变状态
    private void Change(bool isOpen)
    {
        // 播放开门/关门动画
        _animator.SetBool("IsOpen", isOpen);
        // 更改触发器模式
        _collider.isTrigger = isOpen;
    }
    
    // 开门
    public void Open()
    {
        Change(true);
    }
    
    // 关门
    public void Close()
    {
        Change(false);
    }
    
    // 体积碰撞触发器
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果是玩家，则进行房间切换
        if (Player.IsPlayer(other.gameObject))
        {
            StartCoroutine(MoveToNextRoom());
        }
    }
    
    // 让玩家和相机移动到下一个房间
    private IEnumerator MoveToNextRoom()
    {
        // 玩家移动
        Player.transform.position += Vector3.Scale(PlayerMoveVector, new Vector3(2.2f, 1.5f, 1));
        // 相机移动的距离
        var direct = new Vector3(Room.Length * PlayerMoveVector.x, Room.Width * PlayerMoveVector.y, 0);
        // 相机初始位置
        var originPosition = Camera.transform.position;
        // 相机目标位置
        var targetPosition = Camera.transform.position + direct;
        // 初始化计时器
        var clock = new Clock(0.3f);

        // 平滑差值移动
        while (!clock.IsReady())
        {
            clock.Tick();
            Camera.transform.position = Vector3.Lerp(originPosition, targetPosition, clock.t);
            yield return null;
        }
        // 记录玩家移动的信息
        GameManager.Instance.level.OnPlayerMoveToNextRoom(direction);
    }

    // 设置门样式
    public void SetStyle(RoomType type)
    {
        var style = _styleDict[type];
        frame.sprite = style.frame;
        hole.sprite = style.hole;
        leftLeaf.sprite = style.leftLeaf;
        rightLeaf.sprite = style.rightLeaf;
    }
}