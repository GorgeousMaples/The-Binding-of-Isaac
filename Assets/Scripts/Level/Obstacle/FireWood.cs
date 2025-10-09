using UnityEngine;

/**
 * 火炬木
 */
public class FireWood : Obstacle, IAttackable, IHazard
{
    // 火焰渲染器
    [SerializeField] private SpriteRenderer fireRenderer;
    // 火焰碰撞器
    [SerializeField] private CircleCollider2D fireCollider;
    // 火焰动画器
    [SerializeField] private Animator fireAnimator;

    // 火焰伤害
    public int Damage => 1;
    
    // 最大血量
    public int MaxHealth { get; private set; }
    // 血量
    private int _health;
    public int Health
    {
        get => _health; 
        // 确保 HP 不会小于 0
        private set => _health = value >= 0 ? value : 0;
    }

    private void Awake()
    {
        MaxHealth = 4;
        Health = MaxHealth;
    }

    // 攻击方法（来自 IHazard 接口）
    public void Attack(GameObject gameObject)
    {
        // 判断攻击对象是否是玩家
        if (GameManager.IsAttackedPlayer(gameObject))
        {
            GameManager.Instance.player.OnAttacked(Damage, Vector2.zero);
        }
    }

    // 受击方法（来自 IAttackable 接口）
    public void OnAttacked(int damage, Vector2 force = default)
    {
        Health -= damage;
        if (Health == 0)
        {
            // 关闭火焰碰撞器
            fireCollider.enabled = false;
            // 关闭火焰渲染器
            fireRenderer.enabled = false;
        }
        else if (Health <= MaxHealth / 2)
        {
            // 切换到小火焰状态
            fireAnimator.SetTrigger("Small");
            // 火焰碰撞器缩小
            fireCollider.radius = 0.1f;
            fireCollider.offset = new Vector2(0, 0.1f);
        }
        // 闪烁效果
        StartCoroutine(UIManager.FlashRoutine(fireRenderer));
    }
    
    // 触发体积碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attack(collision.gameObject);
    }
}