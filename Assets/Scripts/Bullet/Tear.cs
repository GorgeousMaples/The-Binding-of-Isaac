using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : Bullet
{
    // 实现父类中的抽象属性
    // 泪滴伤害
    protected override int Damage => 1;
    // 发射者（玩家）
    protected override IShooter Shooter => GameManager.Instance.player;

    public override void Initialize()
    {
        IsDestroy = false;
        // 打开体积碰撞
        Collider.enabled = true;
        StartCoroutine(AutoDestroy(1));
    }
    
    private void Update()
    {
        //模拟子弹自然下落
        if (!IsDestroy)
        {
            transform.Translate(0, FallingDistance * Time.deltaTime, 0, Space.World);
        }
    }
    
    private void Destroy()
    {
        IsDestroy = true;
        // 关闭重力
        Rigidbody.gravityScale = 0;
        // 清零速度
        Rigidbody.velocity = Vector2.zero;
        // 关闭体积碰撞
        Collider.enabled = false;
        // 播放销毁动画
        Animator.Play("TearDestroy");
    }

    // 使用协程进行延时自动销毁
    private IEnumerator AutoDestroy(float seconds)
    {
        // 等待指定的秒数
        yield return new WaitForSeconds(seconds);
        Destroy();
    }

    // 销毁动画播放完后执行
    public void OnDestroyAnimationComplete()
    {
        BulletPool.ReturnBullet(this);
    }

    // 触发体积碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Shooter.IsDamageable(collision.gameObject, out var attackedObject))
        {
            attackedObject.OnAttacked(Damage);
            Destroy();
        }
        else if (Obstacle.IsObstacle(collision.gameObject))
        {
             Destroy();
        }
    }
}
