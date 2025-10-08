using UnityEngine;

public class Wall : Obstacle
{
    // 朝向
    public DirectionType direction;
    // 是否是门墙（用于没有生成门的地方）
    public bool isDoorWall;
    
    // 不可破坏
    public bool IsDestructible => false;
    
}