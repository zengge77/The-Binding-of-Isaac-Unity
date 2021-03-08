using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "新的房间布局文件")]
public class RoomLayout : ScriptableObject
{
    public Sprite floor;
    public Sprite tip;
    public Vector2 RewardPosition;
    public bool isGenerateReward;

    public List<GameObject> monsterPrefab = new List<GameObject>();
    public List<Vector2> monsterCoordinate = new List<Vector2>();

    public List<GameObject> obstaclesPreafab = new List<GameObject>();
    public List<Vector2> obstaclesCoordinate = new List<Vector2>();

    public List<GameObject> propPreafab = new List<GameObject>();
    public List<Vector2> propCoordinate = new List<Vector2>();

    public List<SimplePairWithGameObjectVector2> monsterList = new List<SimplePairWithGameObjectVector2>();
    public List<SimplePairWithGameObjectVector2> obstacleList = new List<SimplePairWithGameObjectVector2>();
    public List<SimplePairWithGameObjectVector2> propList = new List<SimplePairWithGameObjectVector2>();
}
