using System;
using UnityEngine;

public abstract class Enemy: Character, IHazard
{
    // 攻击伤害
    public int Damage => 1;
    // 玩家实例
    protected Player Player => GameManager.Instance.player;
    
    // 敌人被击杀事件
    public event Action Killed;
    // 是否被击杀
    public bool IsKilled { get; protected set; }
    
    // 攻击方法
    public virtual void Attack(GameObject target)
    {
        // 判断发生体积碰撞的是否是玩家
        if (GameManager.IsAttackedPlayer(target))
        {
            Player.OnAttacked(Damage, MoveVector);
        }
    }
    
    // 被击杀时的钩子函数
    protected abstract void OnKilled();

    protected override void Start()
    {
        base.Start();
        Initialize();
    }
    
    protected override void Update()
    {
        base.Update();
        if (!IsKilled && Health == 0)
        {
            IsKilled = true;
            OnKilled();
            Killed?.Invoke();
        }
    }
    
    /**
     * 简单追踪算法
     * 适用于没有障碍物的地形或者可以飞行的敌人
     * 直接调用 SimpleTrace() 则返回怪物自身与玩家的距离差向量
     */
    protected Vector2 SimpleTrace(Vector2 position = default, Vector2 correct = default)
    {
        Vector2 selfPosition = position == Vector2.zero ? transform.position : position;
        return (Vector2)Player.transform.position - selfPosition + correct;
    }
    
    // protected Vector2 SimpleTrace(Transform selfTransform = null)
    // {
    //     return Player.transform.position - (selfTransform ?? transform).position;
    // }
}