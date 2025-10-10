using UnityEngine;
using UnityEngine.UI;

// BOSS 类，主要作用就是显示血条
public abstract class Boss : Enemy
{
    private Slider Slider => UIManager.Instance.bossHp;

    protected override void Start()
    {
        base.Start();
        Slider.gameObject.SetActive(true);
        // 把血条拉满
        Slider.value = 1;
    }
    
    protected override void Update()
    {
        base.Update();
        // 每帧让血条丝滑移动
        Slider.value = Mathf.Lerp(Slider.value, 1.0f * Health / MaxHealth, Time.deltaTime * 3);
    }

    protected override void OnKilled()
    {
        // 隐藏血条
        Slider.gameObject.SetActive(false);
    }
}