using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * 角色抽象类
 * 是玩家类、敌对生物类等的基类
 */
public abstract class Character : MonoBehaviour, IAttackable
{
    // —— 角色基础属性 ——
    private int _health;
    private int _shield;
    private float _speed;
    
    // —— 角色基础组件 ——
    protected Rigidbody2D Rigidbody;
    
    // 最大血量
    public int MaxHealth { get; set; }
    // 血量（HP）
    public int Health
    {
        get => _health; 
        // 确保 HP 不会小于 0
        protected set => _health = value >= 0 ? value : 0;
    }
    // 护盾值
    public int Shield
    {
        get => _shield;
        protected set => _shield = value >= 0 ? value : 0;
    }
    // 速度
    public float Speed
    {
        get => _speed; 
        protected set  => _speed = value >= 0 ? value : 1f;
    }
    // 速度增益
    public float SpeedMultiple { get; set; }
    
    // —— 钩子函数 ——
    // 在 Awake 方法中执行
    protected abstract void OnAwake();
    // 在 Start 方法中执行
    protected abstract void OnStart();

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        OnAwake();
    }
    
    private void Start()
    {
        OnStart();
    }

    // 角色初始化
    public virtual void Initialize()
    {
        MaxHealth = 10;
        Health = 10;
        Shield = 0;
        Speed = 1f;
        SpeedMultiple = 1f;
    }
    
    // 被攻击时的闪烁效果
    protected void FlashOnDamage(params SpriteRenderer[] renderers)
    {
        StartCoroutine(FlashRoutine(renderers));
    }
    
    protected IEnumerator FlashRoutine(params SpriteRenderer[] renderers)
    {
        // 原来所有颜色的数组
        var originalColors = new Color[renderers.Length];
        // 受击颜色为红色
        var red = new Color(1, 0.75f, 0.75f, 1);
        for (var i = 0; i < renderers.Length; i++)
        {
            // originalColors[i] = renderers[i].color;
            originalColors[i] = Color.white;
            renderers[i].color = red;
        }
        yield return new WaitForSeconds(0.3f);
        // 恢复为原始颜色
        for (var i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = originalColors[i];
        }
    }

    protected void ReduceHealth(int damage)
    {
        // 攻击剩余点数
        var remain = damage;
        if (Shield > 0)
        {
            Shield -= damage;
            remain -= damage;
        }
        if (remain > 0)
        {
            Health -= remain;
        }
    }
    
    // 被攻击函数的默认实现，子类可以根据自己的情况选择性实现
    public virtual void OnAttacked(int damage, Vector2 force)
    {
        ReduceHealth(damage);
    }
}
