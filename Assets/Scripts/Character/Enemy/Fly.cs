using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 苍蝇
public class Fly : Enemy
{
    // 刚体
    [SerializeField] private Rigidbody2D _rigidbody;
    // 体积碰撞
    [SerializeField] private Collider2D _collider;
    // 渲染器
    [SerializeField] private SpriteRenderer _renderer;
    // 动画器
    [SerializeField] private Animator _animator;

    public override void Initialize()
    {
        base.Initialize();
        MoveCorrect = 0.5f;
        MaxHealth = 3;
        Health = MaxHealth;
    } 
    
    protected override void Update()
    {
        base.Update();
        MoveVector = SimpleTrace().normalized;
        _rigidbody.velocity = Velocity;
    }
    
    protected override void OnKilled()
    {
        MoveCorrect = 0;
        _animator.Play("FlyDie");
    }

    // 动画事件，播放完死亡动画后调用该函数
    private void FinishDeadAnimation()
    {
        gameObject.SetActive(false);
    }
    
    // 受击
    public override void OnAttacked(int damage, Vector2 force)
    {
        ReduceHealth(damage);
        FlashOnDamage(_renderer);
    }
    
    // 触发体积碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attack(collision.gameObject);
    }
}
