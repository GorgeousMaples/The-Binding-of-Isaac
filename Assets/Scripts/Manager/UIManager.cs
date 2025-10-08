using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public PlayerHP playerHP;

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
        playerHP.Initialize();
    }

    public void UpdatePlayerHp()
    {
        playerHP.UpdateView();
    }
}
