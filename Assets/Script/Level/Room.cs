using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [Header("地板")]
    [SerializeField]
    private Transform fllor;
    [SerializeField]
    private Sprite tip;

    [Header("门")]
    [SerializeField]
    private Transform door;
    public List<GameObject> doorList;
    private List<GameObject> activeDoorList = new List<GameObject>();
    public int ActiveDoorCount { get { return activeDoorList.Count; } }
    private List<GameObject> neighboringDoorList = new List<GameObject>();
    //相邻的房间
    private List<Room> neighboringRoomList = new List<Room>();

    [Header("门框")]
    [SerializeField]
    private Sprite bossDoorHole;
    [SerializeField]
    private Sprite bossDoorFrame;
    [SerializeField]
    private Sprite treasureDoorFrame;

    [Header("怪物容器")]
    public Transform monsterContainer;
    [Header("障碍物容器")]
    public Transform ObstaclesContainer;
    [Header("道具容器")]
    public Transform PropContainer;
    [Header("碎片容器")]
    public Transform fragmentContainer;

    //布局文件
    RoomLayout roomLayout;

    //房间类型
    [HideInInspector]
    //public enum RoomType { Start, Normal, Boss, Treasure, Shop }
    public RoomType roomType;

    //方向类型
    //public enum Didirection { Up, Down, Left, Right }

    //坐标
    [HideInInspector]
    public Vector2 coordinate;

    //房间的长和宽
    [HideInInspector]
    public Vector2 roomSize;

    bool isCleared = false;

    //关卡
    private Level level;

    private void Awake()
    {
        Sprite fllorSprite = fllor.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        float roomHeight = fllorSprite.bounds.size.y * transform.localScale.y;
        float roomWidth = fllorSprite.bounds.size.x * transform.localScale.x;
        roomSize = new Vector2(roomHeight, roomWidth);
        level = GameManager.Instance.level;
    }

    /// <summary>
    /// 激活该方向对应的门
    /// </summary>
    /// <param name="didirection"></param>
    public void ActivationDoor(Didirection didirection, Room neighboringRoom, GameObject neighboringDoor)
    {
        GameObject door = doorList[(int)didirection];

        for (int i = 0; i < door.transform.childCount; i++)
        {
            door.transform.GetChild(i).gameObject.SetActive(true);
        }
        activeDoorList.Add(door);
        neighboringRoomList.Add(neighboringRoom);
        neighboringDoorList.Add(neighboringDoor);
    }

    /// <summary>
    /// 打开所有已激活的门
    /// </summary>
    public void OpenActivatedDoor()
    {
        for (int i = 0; i < activeDoorList.Count; i++)
        {
            GameObject door = activeDoorList[i];
            door.GetComponent<BoxCollider2D>().enabled = false;
            door.GetComponent<Animator>().Play("Open");
        }
    }

    /// <summary>
    /// 初始化房间
    /// </summary>
    /// <param name="type"></param>
    public void InitializeRoom()
    {
        roomLayout = level.pools.GetRoomLayout(roomType);
        //改变地板
        for (int i = 0; i < fllor.childCount; i++)
        {
            var go = fllor.GetChild(i);
            go.GetComponent<SpriteRenderer>().sprite = roomLayout.floor;
        }
        if (roomType == RoomType.Start)
        {
            GenerateTraces(tip, Vector2.zero);
        }

        //改变门
        ChangeDoorOutWard();
    }

    /// <summary>
    /// 改变该房间和相邻房间门的样式
    /// </summary>
    private void ChangeDoorOutWard()
    {
        if (roomType == RoomType.Normal || roomType == RoomType.Start) { return; }

        List<GameObject> doors = new List<GameObject>();
        doors.AddRange(activeDoorList);
        doors.AddRange(neighboringDoorList);

        foreach (var door in doors)
        {
            SpriteRenderer doorFrame = door.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer doorHole = door.transform.GetChild(1).GetComponent<SpriteRenderer>();
            switch (roomType)
            {
                case RoomType.Boss:
                    doorFrame.sprite = bossDoorFrame;
                    doorHole.sprite = bossDoorHole;
                    break;
                case RoomType.Treasure:
                    doorFrame.sprite = treasureDoorFrame;
                    break;
                default:
                    break;
            }
        }
    }


    /// <summary>
    /// 根据房间布局文件roomLayout 生成内容
    /// </summary>
    public void GenerateRoomContent(float delaySeconds)
    {
        for (int i = 0; i < roomLayout.obstaclesPreafab.Count; i++)
        {
            GenerateGameObjectWithCoordinate(roomLayout.obstaclesPreafab[i].gameObject, roomLayout.obstaclesCoordinate[i], ObstaclesContainer);
        }

        for (int i = 0; i < roomLayout.monsterPrefab.Count; i++)
        {
            GenerateGameObjectWithCoordinate(roomLayout.monsterPrefab[i], roomLayout.monsterCoordinate[i], monsterContainer);
        }

        for (int i = 0; i < roomLayout.propPreafab.Count; i++)
        {
            GenerateGameObjectWithCoordinate(roomLayout.propPreafab[i], roomLayout.propCoordinate[i], PropContainer);
        }

        CheckOpenDoor();
    }

    /// <summary>
    /// 使用坐标在房间里生成单个物体
    /// </summary>
    private void GenerateGameObjectWithCoordinate(GameObject prefab, Vector2 coordinate, Transform container)
    {
        if (prefab != null)
        {
            int middleX = 13;
            int middleY = 7;
            float width = 0.1385f;
            float heigt = 0.1475f;

            GameObject go = Instantiate(prefab, container);
            Vector2 postiton = new Vector2(-(middleX - coordinate.x) * width, (middleY - coordinate.y) * heigt);
            go.transform.localPosition = postiton;
        }
    }

    /// <summary>
    /// 生成清理房间的奖励品
    /// </summary>
    private void GenerateRoomClearingReward()
    {
        GameObject reward = level.pools.GetRoomClearingReward(roomType);
        GenerateGameObjectWithCoordinate(reward, roomLayout.RewardPosition, PropContainer);
    }

    /// <summary>
    /// 在房间里生成痕迹
    /// </summary>
    public void GenerateTraces(List<Sprite> traces, int num, Vector2 pos, float maxDirection)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject go = new GameObject(traces.ToString());
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            var relic = traces[UnityEngine.Random.Range(0, traces.Count)];
            sr.sprite = relic;
            sr.sortingOrder = GameDate.RENDERERORDER_TRACES;

            go.transform.SetParent(fragmentContainer);
            go.transform.position = pos;
            Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            Vector2 distance = direction * UnityEngine.Random.Range(0, maxDirection);
            go.transform.position += (Vector3)distance;
        }
    }
    public void GenerateTraces(Sprite traces, Vector2 pos)
    {
        GameObject go = new GameObject(traces.ToString());
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = traces;
        sr.sortingOrder = GameDate.RENDERERORDER_TRACES;
        go.transform.SetParent(fragmentContainer);
        go.transform.position = pos;
    }
    /// <summary>
    /// 在房间里生成碎片
    /// </summary>
    public void GenerateFragment(List<Sprite> fragment, int num, Vector2 pos, float maxDirection)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject go = new GameObject(fragment.ToString());
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            var relic = fragment[UnityEngine.Random.Range(0, fragment.Count)];
            sr.sprite = relic;
            sr.sortingOrder = GameDate.RENDERERORDER_FRAGMENT;
            go.transform.SetParent(fragmentContainer);
            go.transform.position = pos;
            StartCoroutine(Flop(go, maxDirection));
        }
    }
    IEnumerator Flop(GameObject gameObject, float maxDirection)
    {
        Vector2 start = gameObject.transform.position;
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector2 distance = direction * UnityEngine.Random.Range(0, maxDirection);
        Vector2 end = start + distance;

        float time = 0;
        float endTime = 0.4f * distance.magnitude;
        while (time < endTime)
        {
            gameObject.transform.position = Vector3.Lerp(start, end, (1f / endTime) * (time += Time.deltaTime));
            yield return null;
        }
    }


    /// <summary>
    /// 检查开门
    /// </summary>
    public void CheckOpenDoor()
    {
        StartCoroutine(DelayCheck());
    }
    IEnumerator DelayCheck()
    {
        yield return new WaitForEndOfFrame();
        if (monsterContainer.childCount == 0 && !isCleared)
        {
            OpenActivatedDoor();
            if (roomLayout.isGenerateReward) { GenerateRoomClearingReward(); }
            isCleared = true;
        }
    }
}
