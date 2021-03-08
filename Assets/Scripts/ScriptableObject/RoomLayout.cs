using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "新的房间布局文件")]
public class RoomLayout : ScriptableObject
{
    public Sprite floor;
    public Sprite tip;

    public bool isGenerateReward;
    public Vector2 RewardPosition;

    public List<SimplePairWithGameObjectVector2> monsterList = new List<SimplePairWithGameObjectVector2>();
    public List<SimplePairWithGameObjectVector2> obstacleList = new List<SimplePairWithGameObjectVector2>();
    public List<SimplePairWithGameObjectVector2> propList = new List<SimplePairWithGameObjectVector2>();
}
