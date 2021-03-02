using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDate
{
    //各类型物体的精灵层级

    //门
    public const int RENDERERORDER_DOOR_HOLE = 1;
    public const int RENDERERORDER_DOOR_PLANE = 2;
    public const int RENDERERORDER_DOOR_FARME = 3;

    //痕迹
    public const int RENDERERORDER_TRACES = 3;
    //碎块
    public const int RENDERERORDER_FRAGMENT = 4;

    //障碍物
    public const int RENDERERORDER_OBSTACLE = 5;

    //道具
    public const int RENDERERORDER_PROP = 6;

    //怪物
    public const int RENDERERORDER_MONSTER = 7;

    //玩家
    public const int RENDERERORDER_PLAYER_BODY = 10;
    public const int RENDERERORDER_BULLET_UNDER = 11;
    public const int RENDERERORDER_PLAYER_HEAD = 12;
    public const int RENDERERORDER_BULLET_ON = 13;

    //各个脚本的加载顺序
    //
}
