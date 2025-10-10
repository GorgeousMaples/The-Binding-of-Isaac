using UnityEngine;

/**
 * 表示有伤害的东西
 * 如玩家的泪滴、部分怪物的血滴、炸弹等
 */
public interface IHazard
{
    // 伤害
    int Damage { get; }
    
    /**
     * 攻击某游戏物体
     * 该方法一般都在体积碰撞事件函数中使用
     * 一般都需要获取 IAttackable 实例，判断该对象是否是自己能攻击的对象，是则调用其 OnAttack 方法
     */
    void Attack(GameObject target);
}