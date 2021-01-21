using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RandomGameObjectTable))]

public class Obstacles : MonoBehaviour
{
    protected Player player;
    protected Room currentRoom;

    private void Awake()
    {
        player = GameManager.Instance.player;
        currentRoom = GameManager.Instance.level.currentRoom;
    }

    protected virtual GameObject GenerateReward()
    {
        GameObject go = null;
        GameObject newGameObject = GetComponent<RandomGameObjectTable>().GetRandomObject();
        if (newGameObject != null)
        {
            go = Instantiate(newGameObject);
            go.transform.SetParent(GameManager.Instance.level.currentRoom.propContainer);
            go.transform.position = transform.position;
        }
        return go;
    }
}
