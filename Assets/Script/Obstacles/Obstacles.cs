using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomGameObjectTable))]
public class Obstacles : MonoBehaviour
{
    protected Player player;
    protected Level level;
    protected Room currentRoom;
    protected GenerateGameObject generateGameObject;

    private void Awake()
    {
        player = GameManager.Instance.player;
        level = GameManager.Instance.level;
        currentRoom = level.currentRoom;
        generateGameObject = level.generateGameObject;
    }

    protected virtual GameObject GenerateReward()
    {
        GameObject randomObject = GetComponent<RandomGameObjectTable>().GetRandomObject();
        if (randomObject == null) { return null; }

        return generateGameObject.GeneratePropInCurrentRoom(randomObject, transform.position);
    }
}
