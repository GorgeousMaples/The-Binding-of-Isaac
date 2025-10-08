using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 玩家预制体
    public Player playerPrefab;
    // 关卡预制体
    public Level levelPrefab;

    // 主相机
    public Camera mainCamera;
    
    // 玩家实体
    [HideInInspector]
    public Player player;
    // 关卡实体
    [HideInInspector]
    public Level level;

    protected override void OnAwake()
    {
        // 根据预制体创建玩家实例
        player = Instantiate(playerPrefab);
        player.Initialize();
        // 根据预制体创建关卡实例
        level = Instantiate(levelPrefab);
        level.Initialize();
    }

    private void Update()
    {
        // 按 ESC 退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
