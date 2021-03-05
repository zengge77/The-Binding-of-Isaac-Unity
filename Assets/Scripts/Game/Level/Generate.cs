using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏过程中生成物体的统一入口
/// </summary>
public class Generate : MonoBehaviour
{
    private Level level;
    private Room currentRoom { get { return level.currentRoom; } }

    private void Awake()
    {
        level = GetComponent<Level>();
    }

    /// <summary>
    /// 使用具体位置在当前房间生成游戏物体
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GenerateGameObjectInCurrentRoom(GameObject prefab, Vector2 position)
    {
        //物体生成后所归属的默认节点
        Transform container = currentRoom.defaultContainer;

        //尝试获取物体类别，不为空便设置到对应节点
        GameItem gameItem = prefab.GetComponent<GameItem>();
        if (gameItem != null)
        {
            switch (gameItem.gameItemType)
            {
                case GameItemType.Prop:
                    container = currentRoom.propContainer;
                    break;
                case GameItemType.Monster:
                    container = currentRoom.monsterContainer;
                    break;
                case GameItemType.Obstacles:
                    container = currentRoom.obstaclesContainer;
                    break;
                default:
                    break;
            }
        }

        return currentRoom.GenerateGameObjectWithPosition(prefab, position, container);
    }

    /// <summary>
    /// 使用具体位置在当前房间生成痕迹
    /// </summary>
    /// <param name="traceSprite"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GenerateTraceInCurrentRoom(Sprite traceSprite, Vector2 position)
    {
        return currentRoom.GenerateTrace(traceSprite, position);
    }

    public void GenerateTracesInCurrentRoom(List<Sprite> traceSprites, int num, Vector2 position, float maxDirection)
    {
        currentRoom.GenerateTraces(traceSprites, num, position, maxDirection);
    }
}