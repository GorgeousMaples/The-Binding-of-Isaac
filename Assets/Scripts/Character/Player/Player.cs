using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("自身属性")]
    // 角色头部
    public Transform head;
    // 角色身体
    public Transform body;
    // 炸弹预制体
    public Bomb bombPrefab;
    
    [Header("子弹池")]
    // 子弹池
    public BulletPool tearPool;
    
    // —— 炸弹相关 ——
    public int BombCount { get; private set; }
    // 炸弹池
    [HideInInspector] public Pool<Bomb> boomPool;
    
    // 渲染器与动画器
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _headRenderer;
    private SpriteRenderer _bodyRenderer;
    private Animator _headAnimator;
    private Animator _bodyAnimator;
    
    // —— 角色移动相关 ——
    // 是否允许移动
    private bool _isAllowedMove = true;
    
    // —— 角色发射泪滴相关 ——
    // 发射按键字典
    private Dictionary<KeyCode, Action> _shootActions;
    // 发射冷却计时器
    private readonly Clock _shotClock = new(0.45f, true);
    
    // —— 角色受击相关 ——
    // 是否无敌
    private bool _isInvincible = false;
    // 无敌时间计时器
    private readonly Clock _invincibleClock = new(3f);
    
    // 死亡事件
    public event Action Dead;
    
    private void Update()
    {
        UpdateShoot();
        // 角色死亡事件判定
        if (Health == 0)
        {
            Dead?.Invoke();
        }
        // 按 E 安置炸弹
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseBomb();
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }
    
    // 获取角色组件
    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody2D>();
        _headRenderer = head.GetComponent<SpriteRenderer>();
        _bodyRenderer = body.GetComponent<SpriteRenderer>();
        _headAnimator = head.GetComponent<Animator>();
        _bodyAnimator = body.GetComponent<Animator>();
    }

    // 角色属性初始化
    protected override void Start()
    {
        base.Start();
        _shootActions =  new Dictionary<KeyCode, Action> {
            { KeyCode.UpArrow,    () => ShootTear("Up", new Vector2(0, 1)) },
            { KeyCode.DownArrow,  () => ShootTear("Down", new Vector2(0, -1)) },
            { KeyCode.LeftArrow,  () => ShootTear("Left", new Vector2(-1, 0)) },
            { KeyCode.RightArrow, () => ShootTear("Right", new Vector2(1, 0)) }
        };
    }

    public override void Initialize()
    {
        base.Initialize();
        Shield = 6;
        BombCount = 10;
        boomPool = new Pool<Bomb>(bombPrefab, GameManager.Instance.transform, 2);
        MoveCorrect = 1.7f;
    }

    // 判断某游戏物体是否是玩家
    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject.GetComponent<Player>() != null;
    }
    
    // —— 角色移动相关 ——
    // 更新角色移动
    private void UpdateMovement()
    {
        if (!_isAllowedMove)
            return;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        // 计算向量方向并归一化（模长变为 1）
        MoveVector = new Vector2(x, y).normalized;
        _rigidbody.velocity = Velocity;
        
        // 设置身体运动动画
        _bodyAnimator.SetFloat("Speed", MoveVector.magnitude);
        _bodyRenderer.flipX = x < 0;
        _bodyAnimator.SetFloat("MoveX", Mathf.Abs(x));
        _bodyAnimator.SetFloat("MoveY", Mathf.Abs(y));
        
        
        // 设置头部运动动画
        _headAnimator.SetFloat("Speed", MoveVector.magnitude);
        _headRenderer.flipX = x < 0;
        _headAnimator.SetFloat("MoveX", Mathf.Abs(x));
        _headAnimator.SetFloat("MoveY", y);
    }
    
    // —— 角色攻击相关 ——
    // 更新角色发射子弹
    private void UpdateShoot()
    {
        _shotClock.Tick();
        // 判断技能是否冷却好
        if (!_shotClock.IsReady())
            return;
        // 依次判断四个按键是否被按下
        foreach (var pair in _shootActions)
        {
            if (!Input.GetKey(pair.Key)) continue;
            // 调用对应的发射函数
            pair.Value();
            break;
        }
    }

    private void ShootTear(string animationName, Vector2 baseVector)
    {
        // 子弹主轴基础力
        int force = 120;
        
        // 从子弹池中获取子弹，并将位置移动到玩家头部
        var bullet = tearPool.TakeBullet(head.position);
        // 子弹初始化
        bullet.Initialize();
        
        // 根据基础方向计算合力向量
        var shootVector = baseVector * force + CorrectVector(baseVector);
        
        // 给子弹添加应力
        bullet.Rigidbody.AddForce(shootVector);
        // 计时器清零（重新开始转 CD）
        _shotClock.Reset();
    }

    // 根据移动向量与射击基础向量构建修正向量
    private Vector2 CorrectVector(Vector2 baseVector)
    {
        // 同向修正
        var sameCorrect = 100;
        // 反向修正
        var oppositeCorrect = -30;
        // 次轴修正
        var minorCorrect = 50;
        // 横向主轴修正
        var mainCorrectX = MoveVector.x * baseVector.x > 0 ? sameCorrect : oppositeCorrect;
        // 纵向主轴修正
        var mainCorrectY = MoveVector.y * baseVector.y > 0 ? sameCorrect : oppositeCorrect;
        
        return new Vector2(
            MoveVector.x * (baseVector.x != 0 ? mainCorrectX : minorCorrect),
            MoveVector.y * (baseVector.y != 0 ? mainCorrectY : minorCorrect)
        ) * Speed * SpeedMultiple;
    }

    private void UseBomb()
    {
        if (BombCount <= 0) return;
        // 将炸弹放置在当前位置并立刻引爆
        boomPool.Take(transform.position).Boom();
    }
    
    // —— 角色受击相关 ——
    public override void OnAttacked(int damage, Vector2 force)
    {
        // 若角色处于无敌状态则不执行
        if (_isInvincible)
            return;
        ReduceHealth(damage);
        UIManager.Instance.UpdatePlayerHp();
        // 受击闪烁后进入无敌时间
        StartCoroutine(AttackedRoutine());
    }

    /**
     * 无敌闪烁状态
     */
    private IEnumerator InvincibleRoutine()
    {
        _isInvincible = true;
        var originalColor = Color.white;
        var flashColor = originalColor;

        while (!_invincibleClock.IsReady())
        {
            // 透明度在 0.5 到 1.0 之间正弦波动
            var alpha = Mathf.Sin(_invincibleClock.t * Mathf.PI * 10) * 0.25f + 0.75f;

            // 应用新的透明度，但保持原有RGB颜色不变
            flashColor.a = alpha;
            _headRenderer.color = flashColor;
            _bodyRenderer.color = flashColor;

            _invincibleClock.Tick();
            
            // 等待下一帧继续循环
            yield return null;
        }

        _headRenderer.color = originalColor;
        _bodyRenderer.color = originalColor;
        _invincibleClock.Reset();
        _isInvincible = false;
    } 

    /**
     * 被攻击后的闪红 + 闪烁
     */
    private IEnumerator AttackedRoutine()
    {
        // 一定要这么写！只能开一个协程！不然这两个颜色会互相干扰！！！
        yield return UIManager.FlashRoutine(_headRenderer, _bodyRenderer);
        yield return InvincibleRoutine();
    }
}
