using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 子弹抽象类
 */
public abstract class Bullet : MonoBehaviour, IHazard
{
    // 是否被销毁
    protected bool IsDestroy = false;
    // 每秒自然下落的距离
    protected float FallingDistance = -0.2f;
    
    // —— 基础属性 ——
    // 刚体
    public Rigidbody2D Rigidbody { get; private set; }
    // 体积碰撞
    protected Collider2D Collider { get; private set; }
    // 动画器
    protected Animator Animator { get; private set; }
    
    // —— 抽象属性 ——
    // 基础伤害
    public abstract int Damage { get; }
    // 攻击方法
    public abstract void Attack(GameObject gameObject);

    // 子弹池
    protected abstract BulletPool BulletPool { get; }

    // 初始化
    public abstract void Initialize();

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
    }
}
