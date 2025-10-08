using UnityEngine;

/**
 * 计时器
 */
public class Clock
{
    // 冷却时间
    private float CoolDownTime { get; }
    // 计时器
    private float Timer { get; set; }
    // 计时比例
    public float t => Timer / CoolDownTime;
    
    // 构造函数
    public Clock(float cd, bool isReady = false)
    {
        CoolDownTime = cd;
        // isReady 表示是否一开始就冷却好了
        Timer = isReady ? cd : 0;
    }
    
    // 是否冷却好
    public bool IsReady()
    {
        return Timer >= CoolDownTime;
    }
    
    // 经过一帧
    public void Tick()
    {
        Timer += Time.deltaTime;
    }
    
    // 重置
    public void Reset()
    {
        Timer = 0;
    }
}