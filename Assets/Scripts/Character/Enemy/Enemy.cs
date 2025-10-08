using UnityEngine;

public abstract class Enemy: Character
{
    // 攻击伤害
    protected int damage = 1;
    // 玩家实例
    protected Player Player => GameManager.Instance.player;
    // 敌人移动向量
    protected Vector2 MoveVector = Vector2.zero;
    
    // 是否接触到的目标是玩家
    protected bool IsAttackedPlayer(GameObject target)
    {
        return target.GetComponent<Player>() != null;
    }

    /// <summary>
    /// 简单追踪算法
    /// 适用于没有障碍物的地形或者可以飞行的敌人
    /// <param name="position"> 自己的位置，不传则用默认值 </param>
    /// <param name="correct"> 修正量，不传则默认零向量 </param>
    /// </summary>
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