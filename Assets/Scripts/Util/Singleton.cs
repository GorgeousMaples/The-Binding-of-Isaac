using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : class
{
    // 私有字段
    private static T _instance;
    // 单例属性
    public static T Instance => _instance;
    
    // 子类初始化方法
    protected virtual void OnAwake() {}
    
    private void Awake()
    {
        _instance = this as T;
        OnAwake();
    }
}