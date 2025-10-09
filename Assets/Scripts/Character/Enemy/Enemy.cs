using System;
using UnityEngine;

public abstract class Enemy: Character
{
    // 攻击伤害
    protected int Damage = 1;
    // 玩家实例
    protected Player Player => GameManager.Instance.player;
    // 敌人移动向量
    protected Vector2 MoveVector = Vector2.zero;
    
    // 敌人被击杀事件
    public event Action Killed;

    // 是否被击杀
    public bool IsKilled { get; protected set; }
    
    protected override void Update()
    {
        base.Update();
        if (Health == 0)
        {
            IsKilled = true;
            OnKilled();
            Killed?.Invoke();
        }
    }

    // 被击杀时的钩子函数
    protected abstract void OnKilled();
    

    
    // 简单追踪算法
    // 适用于没有障碍物的地形或者可以飞行的敌人
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