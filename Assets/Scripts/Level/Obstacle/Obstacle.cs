using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    // 是否可以被破坏
    bool IsDestructible { get; }
    
    // 某游戏物体是否是障碍物
    public static bool IsObstacle(GameObject gameObject)
    {
        return gameObject.GetComponent<Obstacle>() != null;
    }
}