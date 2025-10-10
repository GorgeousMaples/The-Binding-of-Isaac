using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstro : Boss
{
    [SerializeField]
    // 刚体（父组件）
    private Rigidbody2D _rigidbody;
    [SerializeField]
    // 渲染器
    private SpriteRenderer _renderer;
    [SerializeField]
    // 体积碰撞
    private Collider2D _collider;
    
    // —— 跳跃移动相关 ——
    // 怪物是否在移动
    private bool _isMoving;
    // 最大修正系数
    private const float MaxMoveCorrect = 1.5f;

    private void FixedUpdate()
    {
        JumpMove();
    }

    // 怪物的普通移动（跳跃）
    private void JumpMove()
    {
        _rigidbody.velocity = _isMoving ? Velocity : Vector2.zero;
    }

    protected override void OnKilled()
    {
        base.OnKilled();
        gameObject.SetActive(false);
    }
    
    // —— 下面两个方法用于绑定动画事件 ——
    // 怪物开始运动
    public void StartMoving()
    {
        // （0,, 0.31）是修正量，因为怪物和玩家的支撑轴不一样
        var distanceVector = SimpleTrace(transform.position, new Vector2(0, -0.31f));
        // 归一化后就是移动向量
        MoveVector = distanceVector.normalized;
        // 计算模长得到玩家与怪物之间的距离
        var distance = distanceVector.magnitude;
        // 先将距离映射到[0, 1]的区间，0对应 minDistance，1对应 maxDistance
        var t = Mathf.InverseLerp(0.05f, 2f, distance);
        // 使用 SmoothStep 进行平滑插值
        MoveCorrect = Mathf.SmoothStep(0f, 1f, t) * MaxMoveCorrect;
        
        // 根据跳转方向决定是否水平翻转怪物
        _renderer.flipX = MoveVector.x > 0;
        // 关闭体积碰撞
        _collider.enabled = false;
        
        _isMoving = true;
    }
    
    // 怪物停止运动
    public void StopMoving()
    {
        // 打开体积碰撞
        _collider.enabled = true;
        _isMoving = false;
    }
    
    // 怪物受攻击
    public override void OnAttacked(int damage, Vector2 force)
    {
        ReduceHealth(damage);
        FlashOnDamage(_renderer);
    }

    // 触发体积碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Attack(collision.gameObject);
    }
}
