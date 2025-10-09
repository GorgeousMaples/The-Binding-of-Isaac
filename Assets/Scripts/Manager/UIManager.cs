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
    
    // 闪烁效果（一般用于受击）
    public static IEnumerator FlashRoutine(params SpriteRenderer[] renderers)
    {
        // 原来所有颜色的数组
        var originalColors = new Color[renderers.Length];
        // 受击颜色为红色
        var red = new Color(1, 0.75f, 0.75f, 1);
        for (var i = 0; i < renderers.Length; i++)
        {
            // originalColors[i] = renderers[i].color;
            originalColors[i] = Color.white;
            renderers[i].color = red;
        }
        yield return new WaitForSeconds(0.3f);
        // 恢复为原始颜色
        for (var i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = originalColors[i];
        }
    }
}
