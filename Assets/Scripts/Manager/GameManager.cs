using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 玩家预制体
    public Player playerPrefab;

    // 主相机
    public Camera mainCamera;
    
    // 玩家实体
    [HideInInspector]
    public Player player;

    protected override void OnAwake()
    {
        // 根据预制体创建玩家实例
        player = Instantiate(playerPrefab);
        // 初始化玩家
        player.Initialize();
    }
}
