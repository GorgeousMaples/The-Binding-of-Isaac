using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 血量图标样式
public enum HeartType
{
    Full, Half, Void, Soul, SoulHalf
}

// 血量图标
public class Heart : MonoBehaviour
{
    // 图标
    public Image image;
    // 类型
    private HeartType _type;

    // 设置血量样式
    public void SetStyle(HeartType type)
    {
        _type = type;
        image.sprite = UIManager.HeartStyleDict[type].sprite;
    }
}