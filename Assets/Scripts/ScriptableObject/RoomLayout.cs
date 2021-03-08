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

    public void AddMonster()
    {
        monsterPrefab.Add(null);
        monsterCoordinate.Add(Vector2.zero); 
    }
    public void RemoveMonster(int index)
    {
        monsterPrefab.RemoveAt(index);
        monsterCoordinate.RemoveAt(index);
    }
    public void AddObstacles()
    {
        obstaclesPreafab.Add(null);
        obstaclesCoordinate.Add(Vector2.zero);
    }
    public void RemoveObstacles(int index)
    {
        obstaclesPreafab.RemoveAt(index);
        obstaclesCoordinate.RemoveAt(index);
    }
    public void AddProp()
    {
        propPreafab.Add(null);
        propCoordinate.Add(Vector2.zero);
    }
    public void RemoveProp(int index)
    {
        propPreafab.RemoveAt(index);
        propCoordinate.RemoveAt(index);
    }

    public void AutoCompletion()
    {
        while (monsterPrefab.Count != monsterCoordinate.Count)
        {
            if (monsterPrefab.Count > monsterCoordinate.Count)
            {
                monsterCoordinate.Add(Vector2.zero);
            }
            if (monsterPrefab.Count < monsterCoordinate.Count)
            {
                monsterPrefab.Add(null);
            }
        }
    }
}
