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
    
    // 最大血量
    public int MaxHealth { get; protected set; }
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
    // —— 角色移动相关 ——
    // 速度
    public float Speed
    {
        get => _speed; 
        protected set  => _speed = value >= 0 ? value : 1f;
    }
    // 速度增益
    public float SpeedMultiple { get; set; }
    // 移动向量
    protected Vector2 MoveVector;
    // 移动修正量
    protected float MoveCorrect = 1f;
    // 速度矢量
    protected Vector2 Velocity => MoveVector * ((1.0f + 0.5f * Speed) * SpeedMultiple * MoveCorrect);
    
    // 改为虚函数，子类重写时需要加 base
    protected virtual void Awake() {}

    protected virtual void Start() { }
    protected virtual void Update() {}

    // 角色初始化
    public virtual void Initialize()
    {
        MaxHealth = 10;
        Health = MaxHealth;
        Shield = 0;
        Speed = 1f;
        SpeedMultiple = 1f;
    }
    
    // 被攻击时的闪烁效果
    protected void FlashOnDamage(params SpriteRenderer[] renderers)
    {
        StartCoroutine(UIManager.FlashRoutine(renderers));
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
