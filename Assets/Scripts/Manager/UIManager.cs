using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 玩家血条
    public PlayerHP playerHp;
    // Boss血条
    public Slider bossHp;

    // 血量样式的字典
    public static readonly Dictionary<HeartType, HeartStyle> HeartStyleDict = new();
    
    protected override void OnAwake()
    {
        // 导入样式
        var allStyles = new List<HeartStyle>(Resources.LoadAll<HeartStyle>("HeartStyle"));
        allStyles.ForEach(style => HeartStyleDict[style.type] = style);
    }

    private void Start()
    {
        playerHp.Initialize();
    }

    public void UpdatePlayerHp()
    {
        playerHp.UpdateView();
    }
}
