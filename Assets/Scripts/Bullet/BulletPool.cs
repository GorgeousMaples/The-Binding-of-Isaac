using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 子弹池
 */
public class BulletPool : MonoBehaviour
{
    // 子弹预制体
    public Bullet bulletPrefab;
    
    // 子弹池
    private Pool<Bullet> _pool;

    private void Awake()
    {
        _pool = new Pool<Bullet>(bulletPrefab, transform);
    }

    public Bullet TakeBullet(Vector3 position = default) => _pool.Take(position);
    
    public void ReturnBullet(Bullet bullet) => _pool.Return(bullet);
}
