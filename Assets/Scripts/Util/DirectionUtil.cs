using System.Collections.Generic;

// 方向类型枚举
public enum DirectionType
{
    Up, Down, Left, Right
}

public class DirectionUtil
{
    // 所有方向的数组
    public static readonly DirectionType[] All =
    {
        DirectionType.Up, 
        DirectionType.Down, 
        DirectionType.Left, 
        DirectionType.Right
    };
    
    // 坐标偏移映射
    public static readonly Dictionary<DirectionType, (int, int)> Offset = new()
    {
        [DirectionType.Up] = (0,  1),
        [DirectionType.Down] = (0, -1),
        [DirectionType.Left] = (-1,  0),
        [DirectionType.Right] = (1,  0)
    };
    
    // 反向映射
    public static DirectionType GetOpposite(DirectionType dir) => dir switch
    {
        DirectionType.Up => DirectionType.Down,
        DirectionType.Down => DirectionType.Up,
        DirectionType.Left => DirectionType.Right,
        DirectionType.Right => DirectionType.Left,
        _ => DirectionType.Up
    };
}