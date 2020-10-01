using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    [Header("房间池")]
    List<RoomLayout> startRoom = new List<RoomLayout>();
    List<RoomLayout> normalRoom = new List<RoomLayout>();
    List<RoomLayout> bossRoom = new List<RoomLayout>();
    List<RoomLayout> treasureRoom = new List<RoomLayout>();
    List<RoomLayout> shopRoom = new List<RoomLayout>();
    string[] RoomLayoutFileFolderPath = new[]
{
        "ScriptableObject/RoomLayout/StartRoom",
        "ScriptableObject/RoomLayout/NormalRoom",
        "ScriptableObject/RoomLayout/BossRoom",
        "ScriptableObject/RoomLayout/TreasureRoom",
        "ScriptableObject/RoomLayout/ShopRoom",
    };

    [Header("道具池")]
    [SerializeField]
    private Prop defaultProp;//默认道具，道具池为空时返回该道具

    [Space(10)]
    [SerializeField]
    private PropPool TreasureRoomPropPool;
    private List<Prop> TreasureRoomPropList = new List<Prop>();
    [SerializeField]
    private PropPool GoldenChestPropPool;
    private List<Prop> GoldenChestPropList = new List<Prop>();
    [SerializeField]
    private PropPool BossRoomPropPool;
    private List<Prop> BossRoomPropList = new List<Prop>();
    [SerializeField]
    private PropPool ShopPropPool;
    private List<Prop> ShopPropList = new List<Prop>();

    [Header("Boss房清空奖励")]
    [SerializeField]
    private GameObject bossRoomClearingReward;
    [Header("普通房清空奖励")]
    [SerializeField]
    private RandomGameObjectScriptable clearingReward;
    [Header("拾取物货物")]
    [SerializeField]
    private RandomGameObjectScriptable pickupGoods;

    void Awake()
    {
        //填充
        startRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[0]));
        normalRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[1]));
        bossRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[2]));
        treasureRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[3]));
        shopRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[4]));

        GoldenChestPropList.AddRange(GoldenChestPropPool.propList);
        TreasureRoomPropList.AddRange(TreasureRoomPropPool.propList);
        BossRoomPropList.AddRange(BossRoomPropPool.propList);
        ShopPropList.AddRange(ShopPropPool.propList);
    }

    /// <summary>
    /// 从房间池中获取房间布局文件
    /// </summary>
    /// <returns></returns>
    public RoomLayout GetRoomLayout(RoomType type)
    {
        //测试模式，返回测用的布局文件
        if (GameManager.Instance.isUseTestMod)
        {
            switch (type)
            {
                case RoomType.Start:
                    return GameManager.Instance.startRoomTestSample;
                case RoomType.Normal:
                    return GameManager.Instance.normalRoomTestSample;
                case RoomType.Boss:
                    return GameManager.Instance.bossRoomTestSample;
                case RoomType.Treasure:
                    return GameManager.Instance.treasureRoomTestSample;
                default:
                    break;
            }
        }

        //正常模式，返回指定的布局文件
        switch (type)
        {
            case RoomType.Start:
                return GetRandomRoomLayout(startRoom, false);
            case RoomType.Normal:
                if (normalRoom.Count == 0)
                {
                    normalRoom.AddRange(Resources.LoadAll<RoomLayout>(RoomLayoutFileFolderPath[1]));
                }
                return GetRandomRoomLayout(normalRoom, true);
            case RoomType.Boss:
                return GetRandomRoomLayout(bossRoom, false);
            case RoomType.Treasure:
                return GetRandomRoomLayout(treasureRoom, false);
            case RoomType.Shop:
                return GetRandomRoomLayout(shopRoom, false);
            default:
                return null;
        }

    }
    RoomLayout GetRandomRoomLayout(List<RoomLayout> list, bool isRemove)
    {
        RoomLayout go;
        int index = Random.Range(0, list.Count);
        go = list[index];
        if (isRemove) { list.RemoveAt(index); }
        return go;
    }

    /// <summary>
    /// 从各个道具池中获取道具
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Prop GetProp(PropPoolType type)
    {
        Prop prop;
        switch (type)
        {
            case PropPoolType.TreasureRoom:
                prop = GetRamdomProp(TreasureRoomPropList);
                break;
            case PropPoolType.GoldenChest:
                prop = GetRamdomProp(GoldenChestPropList);
                break;
            case PropPoolType.BossRoom:
                prop = GetRamdomProp(BossRoomPropList);
                break;
            case PropPoolType.Shop:
                prop = GetRamdomProp(ShopPropList);
                break;
            default:
                prop = null;
                break;
        }
        return prop;
    }
    private Prop GetRamdomProp(List<Prop> list)
    {
        if (list.Count == 0)
        {
            if (TreasureRoomPropList.Count == 0)
            {
                return defaultProp;
            }
            return GetRamdomProp(TreasureRoomPropList);
        }

        int index = Random.Range(0, list.Count);
        Prop go = list[index];
        list.RemoveAt(index);
        return go;
    }

    /// <summary>
    /// 获取清空房间的奖励
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetRoomClearingReward(RoomType type)
    {
        GameObject reward = null;
        switch (type)
        {
            case RoomType.Normal:
                int luck = GameManager.Instance.player.Luck;
                reward = clearingReward.GetRandomObject(luck * 5);
                break;
            case RoomType.Boss:
                reward = bossRoomClearingReward;
                break;
            default:
                break;
        }
        return reward;
    }

    /// <summary>
    /// 获取拾取物商品
    /// </summary>
    /// <returns></returns>
    public GameObject GetPickupGoods()
    {
        return pickupGoods.GetRandomObject();
    }
}
