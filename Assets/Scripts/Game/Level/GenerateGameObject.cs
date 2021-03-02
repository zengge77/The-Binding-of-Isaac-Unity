using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGameObject : MonoBehaviour
{
    private Level level;
    private Room currentRoom { get { return level.currentRoom; } }

    private void Awake()
    {
        level = GetComponent<Level>();
    }

    public GameObject GenerateMonsterInCurrentRoom(GameObject prefab, Vector2 position)
    {
        return currentRoom.GenerateGameObjectWithPosition(prefab, position, currentRoom.monsterContainer); ;
    }

    public GameObject GenerateObstaclesInCurrentRoom(GameObject prefab, Vector2 position)
    {
        return currentRoom.GenerateGameObjectWithPosition(prefab, position, currentRoom.obstaclesContainer);
    }

    public GameObject GeneratePropInCurrentRoom(GameObject prefab, Vector2 position)
    {
        return currentRoom.GenerateGameObjectWithPosition(prefab, position, currentRoom.propContainer);
    }


    public GameObject GenerateTraceInCurrentRoom(Sprite traceSprite, Vector2 position)
    {
        return currentRoom.GenerateTrace(traceSprite, position);
    }

    public void GenerateTracesInCurrentRoom(List<Sprite> traceSprites, int num, Vector2 position, float maxDirection)
    {
        currentRoom.GenerateTraces(traceSprites, num, position, maxDirection);
    }
}
