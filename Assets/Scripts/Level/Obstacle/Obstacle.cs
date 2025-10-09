using UnityEngine;

/**
 * 障碍物抽象类
 * 目前的作用就是提供一个标记（例如子弹碰到障碍物触发 Destroy 方法）
 */
public abstract class Obstacle : MonoBehaviour
{
    // 某游戏物体是否是障碍物
    public static bool IsObstacle(GameObject gameObject)
    {
        return gameObject.GetComponent<Obstacle>() != null;
    }
}