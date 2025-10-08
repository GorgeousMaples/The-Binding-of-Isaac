using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 血量条
public class PlayerHP : MonoBehaviour
{
    // 血量图标预制体
    public Heart heartPrefab;
    
    // 血量池
    private Pool<Heart> _heartPool;
    
    private Player _player;

    public void Initialize()
    {
        _player = GameManager.Instance.player;
        var total = Mathf.CeilToInt(_player.MaxHealth *3 / 4f);
        _heartPool = new Pool<Heart>(heartPrefab, transform, total);
        UpdateView();
    }

    // 更新图标显示
    public void UpdateView()
    {
        _heartPool.Reset();
        for (var i = 0; i < _player.Health / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Full);
        }
        for (var i = 0; i < _player.Health % 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Half);
        }
        for (var i = 0; i < (_player.MaxHealth - _player.Health) / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Void);
        }
        for (var i = 0; i < _player.Shield / 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.Soul);
        }
        for (var i = 0; i < _player.Shield % 2; i++)
        {
            _heartPool.Take().SetStyle(HeartType.SoulHalf);
        }
    }
}
