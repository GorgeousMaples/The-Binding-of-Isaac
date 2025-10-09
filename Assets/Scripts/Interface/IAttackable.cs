using System.Collections;
using UnityEngine;

/**
 * 可受击对象
 * 玩家、敌人、部分可被攻击的物品（碎石、火炬木等）
 */
public interface IAttackable
{
    // 最大血量
    int MaxHealth { get; }
    // 当前血量
    int Health { get; }
    
    /**
     * 被攻击方法
     * 用于自身被攻击后的一些逻辑处理
     */
    void OnAttacked(int damage, Vector2 force = default);
}