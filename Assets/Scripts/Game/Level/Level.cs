using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    #region 房间相关
    [Header("房间相关")]
    [SerializeField]
    private Room roomPrefab;
    [SerializeField]
    private int roomNum;
    [HideInInspector]
    public Room[,] roomArray = new Room[20, 20];
    private List<Room> haveBeenToRoomList = new List<Room>();
    [HideInInspector]
    public Room currentRoom;
    #endregion

    #region 功能类
    [HideInInspector]
    public Pools pools;
    [HideInInspector]
    public Generate generate;
    #endregion

    #region 其他
    private Player player;
    private UIManager UI;
    #endregion

    private void Awake()
    {
        pools = GetComponent<Pools>();
        generate = GetComponent<Generate>();
    }

    private void Start()
    {
        UI = UIManager.Instance;
        player = GameManager.Instance.player;
        CreateRooms();
        UI.miniMap.CreatMiniMap();
        StartCoroutine(MoveToNextRoom(Vector2.zero));
    }

    /// <summary>
    /// 创建所有的房间
    /// </summary>
    private void CreateRooms()
    {
        //储存备选生成房间的位置列表
        List<Vector2> alternativeRoomList = new List<Vector2>();
        List<Vector2> hasBeenRemoveRoomList = new List<Vector2>();

        //单门房间列表
        List<Room> singleDoorRoomList = new List<Room>();

        while (singleDoorRoomList.Count < 3)
        {
            //清空已生成的房间
            Array.Clear(roomArray, 0, roomArray.Length);
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            //清空相关数据
            alternativeRoomList.Clear();
            hasBeenRemoveRoomList.Clear();
            singleDoorRoomList.Clear();

            //创建起始房间
            int outsetX = roomArray.GetLength(0) / 2;
            int outsetY = roomArray.GetLength(1) / 2;
            Room lastRoom = roomArray[outsetX, outsetY] = CreateRoom(new Vector2(outsetX, outsetY));
            currentRoom = lastRoom;

            //创建其他房间
            Action<int, int> action = (newX, newY) =>
            {
                Vector2 coordinate = new Vector2(newX, newY);
                if (roomArray[newX, newY] == null)
                {
                    if (alternativeRoomList.Contains(coordinate))
                    {
                        alternativeRoomList.Remove(coordinate);
                        hasBeenRemoveRoomList.Add(coordinate);
                    }
                    else if (!hasBeenRemoveRoomList.Contains(coordinate))
                    {
                        alternativeRoomList.Add(coordinate);
                    }
                }
            };

            for (int i = 1; i < roomNum; i++)
            {
                int x = (int)lastRoom.coordinate.x; int y = (int)lastRoom.coordinate.y;

                action(x + 1, y);
                action(x - 1, y);
                action(x, y - 1);
                action(x, y + 1);

                Vector2 newRoomCoordinate = alternativeRoomList[UnityEngine.Random.Range(0, alternativeRoomList.Count)];
                lastRoom = roomArray[(int)newRoomCoordinate.x, (int)newRoomCoordinate.y] = CreateRoom(newRoomCoordinate);
                alternativeRoomList.Remove(newRoomCoordinate);
            }

            //打通门
            LinkDoors();

            //计算单门房间数量
            foreach (Room room in roomArray)
            {
                if (room != null && room.ActiveDoorCount == 1 && room != currentRoom)
                {
                    singleDoorRoomList.Add(room);
                }
            }
        }

        //设置房间类型
        SetRoomsType(singleDoorRoomList);
    }
    private Room CreateRoom(Vector2 coordinate)
    {
        Room newRoom = Instantiate(roomPrefab, transform);
        newRoom.coordinate = coordinate;

        int x = (int)coordinate.x - roomArray.GetLength(0) / 2;
        int y = (int)coordinate.y - roomArray.GetLength(1) / 2;
        float roomHeight = 2 * newRoom.roomHeight;
        float roomWidth = 2 * newRoom.roomWidth;
        newRoom.transform.position = new Vector2(y * roomWidth, x * roomHeight);

        return newRoom;
    }

    /// <summary>
    /// 打通各个房间相连的门，并记录相连信息
    /// </summary>
    private void LinkDoors()
    {
        foreach (Room room in roomArray)
        {
            if (room != null)
            {
                int x = (int)room.coordinate.x; int y = (int)room.coordinate.y;
                if (roomArray[x + 1, y] != null)
                {
                    Room neighboringRoom = roomArray[x + 1, y];
                    GameObject neighboringDoor = neighboringRoom.doorList[1];
                    room.ActivationDoor(Didirection.Up, neighboringRoom, neighboringDoor);
                }
                if (roomArray[x - 1, y] != null)
                {
                    room.ActivationDoor(Didirection.Down, roomArray[x - 1, y], (roomArray[x - 1, y].doorList[0]));
                }
                if (roomArray[x, y - 1] != null)
                {
                    room.ActivationDoor(Didirection.Left, roomArray[x, y - 1], roomArray[x, y - 1].doorList[3]);
                }
                if (roomArray[x, y + 1] != null)
                {
                    room.ActivationDoor(Didirection.Right, roomArray[x, y + 1], roomArray[x, y + 1].doorList[2]);
                }
            }
        }
    }

    /// <summary>
    /// 设置各个房间的类型
    /// </summary>
    private void SetRoomsType(List<Room> singleDoorRoomList)
    {
        //先全部设为普通
        foreach (Room room in roomArray)
        {
            if (room != null)
            {
                room.roomType = RoomType.Normal;
            }
        }

        //宝藏
        for (int i = 0; i < singleDoorRoomList.Count - 2; i++)
        {
            singleDoorRoomList[i].roomType = RoomType.Treasure;
        }

        //Boss
        singleDoorRoomList[singleDoorRoomList.Count - 1].roomType = RoomType.Boss;
        //商店
        singleDoorRoomList[singleDoorRoomList.Count - 2].roomType = RoomType.Shop;
        //起始
        currentRoom.roomType = RoomType.Start;

        //初始化
        foreach (Room room in roomArray)
        {
            if (room != null)
            {
                room.Initialize();
            }
        }
    }

    /// <summary>
    /// 更新玩家所在房间
    /// </summary>
    /// <param name="MoveDirection">移动方向，使用Vector2默认的几个类型</param>
    public IEnumerator MoveToNextRoom(Vector2 MoveDirection)
    {
        Camera mainCamera = GameManager.Instance.myCamera;
        float delaySeconds = 0.3f;

        int x = (int)currentRoom.coordinate.x + (int)MoveDirection.y;
        int y = (int)currentRoom.coordinate.y + (int)MoveDirection.x;
        currentRoom = roomArray[x, y];

        //如果没去过该房间便生成房间内容
        if (!haveBeenToRoomList.Contains(currentRoom))
        {
            currentRoom.GenerateRoomContent(delaySeconds);
            haveBeenToRoomList.Add(currentRoom);
        }
        UI.miniMap.UpdateMiniMap(MoveDirection);

        //暂停并移动玩家
        player.PlayerPause();
        player.transform.position += (Vector3)MoveDirection;

        //移动镜头
        Vector3 originPos = mainCamera.transform.position;
        Vector3 targetPos = currentRoom.transform.position;
        targetPos.z += mainCamera.transform.position.z;
        float time = 0;
        while (time <= delaySeconds)
        {
            mainCamera.transform.position = Vector3.Lerp(originPos, targetPos, (1 / delaySeconds) * (time += Time.deltaTime));
            yield return null;
        }

        //恢复玩家暂停
        player.PlayerQuitPause();
    }
}
