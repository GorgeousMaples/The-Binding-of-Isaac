using UnityEngine;

[CreateAssetMenu(menuName = "Heart Style")]
public class HeartStyle : ScriptableObject
{
    // 血量类型
    public HeartType type;
    // 图标样式
    public Sprite sprite;
}