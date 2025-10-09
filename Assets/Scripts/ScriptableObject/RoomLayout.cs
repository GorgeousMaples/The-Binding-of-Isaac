using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Layout")]
public class RoomLayout : ScriptableObject
{
    // 房间类型
    public RoomType type;
    
    // 地板样式
    public Sprite floor;
    // 提示样式（目前只有开始关卡会用到这个）
    public Sprite tip;
    
    // 敌人列表
    public List<GameObjectWithPosition<Enemy>> enemyList = new();
    // 障碍物列表
    public List<GameObjectWithPosition<Obstacle>> obstacleList = new();
    // // 道具列表
    // public List<GameObjectWithPosition> propList = new();

    // 游戏物品与位置
    [System.Serializable]
    public class GameObjectWithPosition<T> where T : MonoBehaviour
    {
        // 物品类型
        public T gameObject;
        // 位置
        public Vector2 position;

        public GameObjectWithPosition(T obj,  Vector2 pos)
        {
            gameObject = obj;
            position = pos;
        }
    }
}