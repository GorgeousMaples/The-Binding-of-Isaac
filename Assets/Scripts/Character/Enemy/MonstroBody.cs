using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 这个脚本唯一的作用就是设定两个动画事件，因为 Monstro 的动画是绑定在它的 Body 上的
public class MonstroBody : MonoBehaviour
{
    public Monstro Monstro;

    private void StartMoving() => Monstro.StartMoving();
    
    private void StopMoving() => Monstro.StopMoving();
}
