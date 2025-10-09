using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Initialize();
    }

    private void Update()
    {
        // 按 ESC 退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Initialize()
    {
        // 根据预制体创建玩家实例
        player = Instantiate(playerPrefab, transform);
        player.Initialize();
        // 订阅事件，角色死亡时触发重启操作
        player.Dead += RestartGame;
        // 根据预制体创建关卡实例
        level = Instantiate(levelPrefab);
        level.Initialize();
    }

    // 实例化敌人（让所有的敌人都归 GameManager 控制）
    public Enemy InstantiateEnemy(Enemy enemyPrefab)
    {
        var enemy = Instantiate(enemyPrefab, transform);
        return enemy;
    }
    
    private void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        // 先销毁原有的物件
        Destroy(player.gameObject);
        player = null;
        Destroy(level.gameObject);
        level = null;
        // 等待一帧再执行，因为销毁操作实在帧末执行的
        yield return null;
        Initialize();
        // 重绘角色血量
        UIManager.Instance.UpdatePlayerHp();
        // 移动主相机
        var originPos = mainCamera.transform.position;
        mainCamera.transform.position = new Vector3(0, 0, originPos.z);
    }
}
