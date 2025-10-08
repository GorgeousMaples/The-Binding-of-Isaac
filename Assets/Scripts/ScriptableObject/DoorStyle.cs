using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Door Style")]
public class DoorStyle : ScriptableObject
{
    // 房间类型
    public RoomType type;
    // 框架样式
    public Sprite frame;
    // 门洞样式
    public Sprite hole;
    // 门扇样式
    public Sprite leftLeaf;
    public Sprite rightLeaf;
}