using UnityEngine;

/**
 * 射手接口
 * 如玩家（可发射泪滴）、部分怪物（可发射血滴）
 */
public interface IShooter
{
    // 子弹池
    BulletPool BulletPool { get; }
    
    // 该游戏物体是否可以被攻击
    bool IsDamageable(GameObject gameObject, out IAttackable attackedObject);
}