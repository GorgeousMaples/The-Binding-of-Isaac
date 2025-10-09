using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 血量条
public class PlayerHP : MonoBehaviour
{
    // 血量图标预制体
    public HeartIcon heartIconPrefab;
    
    // 血量池
    private Pool<HeartIcon> _heartPool;
    
    private Player Player => GameManager.Instance.player;

    public void Initialize()
    {
        var total = Mathf.CeilToInt(Player.MaxHealth *3 / 4f);
        _heartPool = new Pool<HeartIcon>(heartIconPrefab, transform, total);
        UpdateView();
    }

    // 更新图标显示
    public void UpdateView()
    {
        _heartPool.Reset();
        for (var i = 0; i < Player.Health / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Full);
        }
        for (var i = 0; i < Player.Health % 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Half);
        }
        for (var i = 0; i < (Player.MaxHealth - Player.Health) / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Void);
        }
        for (var i = 0; i < Player.Shield / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Soul);
        }
        for (var i = 0; i < Player.Shield % 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.SoulHalf);
        }
    }
}
