using System;
using System.Collections;
using UnityEngine;

// 炸弹掉落物
public class Bomb : MonoBehaviour, IHazard
{
    // 爆炸范围碰撞器
    [SerializeField] private Collider2D boomCollider;
    // 渲染器
    [SerializeField] private SpriteRenderer spriteRenderer;
    // 影子渲染器
    [SerializeField] private SpriteRenderer shadowRenderer;
    // 动画器
    [SerializeField] private Animator animator;
    
    // 爆炸伤害
    public int Damage => 10;
    // 所属炸弹池
    private Pool<Bomb> _pool => GameManager.Instance.player.boomPool;

    // 引爆炸弹
    public void Boom()
    {
        StartCoroutine(BoomFlashRoutine());
    }

    // 爆炸闪烁
    private IEnumerator BoomFlashRoutine()
    {
        // 初始颜色
        var originalColor = Color.white;
        var redColor = Color.red;
        
        // 计时器，3s 后爆炸
        var clock = new Clock(2.5f);

        while (!clock.IsReady())
        {
            // 透明度在 0.5 到 1.0 之间正弦波动
            var t = Mathf.Sin(clock.t * Mathf.PI * 10) * 0.5f + 0.5f;
            // 在原始色与红色之间插值
            spriteRenderer.color = Color.Lerp(originalColor, redColor, t);
            clock.Tick();
            
            // 等待下一帧继续循环
            yield return null;
        }

        spriteRenderer.color = originalColor;
        // 播放爆炸动画
        animator.SetBool("IsBoom", true);
    }

    // 动画事件，开始爆炸
    private void StartBoom()
    {
        // 打开爆炸范围碰撞器
        boomCollider.enabled = true;
        // 关闭影子
        shadowRenderer.enabled = false;
    }

    // 动画事件，结束爆炸
    private void StopBoom()
    {
        // 回收自己
        _pool.Return(this);
        // 重置为未爆炸状态
        animator.SetBool("IsBoom", false);
        // 关闭爆炸范围碰撞器
        boomCollider.enabled = false;
        // 打开影子
        shadowRenderer.enabled = true;
    }
    
    // 攻击方法（来自接口 IHazard）
    public void Attack(GameObject gameObject)
    {
        if (GameManager.IsAttackedEnemy(gameObject, out var attackedObject))
        {
            attackedObject.OnAttacked(Damage);
        }
        // 未来还有可能会有炸弹破坏某些易碎障碍物的逻辑
    }

    // 爆炸体积碰撞判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other.gameObject);
    }
}