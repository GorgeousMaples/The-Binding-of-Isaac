using System.Collections.Generic;
using UnityEngine;

/**
 * 对象池
 */
public class Pool<T> where T : MonoBehaviour
{
    // 预制体
    private T _prefab;
    // 需要挂载的父对象
    private Transform _parent;
    
    // 可用对象池队列（空闲、可被取用的对象）
    private readonly Queue<T> _availablePool = new();
    // 活跃对象池集合（已被取走、正在使用中的对象）
    private readonly HashSet<T> _activePool = new();

    // 默认初始生成数
    private const int DefaultSize = 5;
    // 每次扩充大小
    private const int ExpandSize = 2;

    public Pool(T prefab, Transform parent, int size = DefaultSize)
    {
        _prefab = prefab;
        _parent = parent;
        AddToPool(DefaultSize);
    }

    // 取出子弹
    public T Take()
    {
        // 若对象池中可用对象不够则先扩充
        if (_availablePool.Count <= 0)
            ExpandPool();
        // 从对象池中取出一个对象并激活
        var obj = _availablePool.Dequeue();
        _activePool.Add(obj);
        obj.gameObject.SetActive(true);
        // 放到对象栏的最后一位
        obj.transform.SetAsLastSibling();
        return obj;
    }

    // 回收子弹
    public void Return(T obj)
    {
        _availablePool.Enqueue(obj);
        _activePool.Remove(obj);
        obj.gameObject.SetActive(false);
    }

    // 重置对象池，让所有对象恢复待取状态
    public void Reset()
    {
        // 创建副本避免修改集合异常
        var activeObjects = new List<T>(_activePool);
    
        foreach (var obj in activeObjects)
        {
            // 将活跃对象归还到可用池
            Return(obj);
        }
    }
    
    // 填充
    private void AddToPool(int number)
    {
        for (var i = 0; i < number; i++)
        {
            var obj = Object.Instantiate(_prefab, _parent);
            _availablePool.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }
    
    // 扩充子弹池
    private void ExpandPool()
    {
        AddToPool(ExpandSize);
    }
}