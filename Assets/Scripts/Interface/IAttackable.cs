using System.Collections;
using UnityEngine;

/**
 * 可受击对象
 */
public interface IAttackable
{
    /// <summary>
    /// 被攻击方法
    /// </summary>
    /// <param name="damage"> 伤害 </param>
    /// <param name="force"> 受力矢量 </param>
    void OnAttacked(int damage, Vector2 force = default);
}