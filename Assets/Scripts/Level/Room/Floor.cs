using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField]
    // 渲染器
    private SpriteRenderer renderer;

    // 设置地板样式
    public void SetLayout(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}
