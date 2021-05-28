using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包MVC模式--Model层。保存背包物品数据，提供数据操作方法
/// </summary>
public class ItemModel
{
    private List<ItemInformation> itemInfoLsit;//记录拥有的道具
    private Dictionary<ItemInformation, int> itemInfoDict;//记录道具的数量

    public ItemModel()
    {
        itemInfoLsit = new List<ItemInformation>();
        itemInfoDict = new Dictionary<ItemInformation, int>();
    }

    /// <summary>
    /// 添加道具信息
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        ItemInformation itemInfo = item.itemInformation;
        if (!itemInfoDict.ContainsKey(itemInfo))
        {
            itemInfoLsit.Add(itemInfo);
            itemInfoDict.Add(itemInfo, 1);
        }
        else
        {
            itemInfoDict[itemInfo] += 1;
        }
        UIManager.Instance.pausePanel.backPack.UpdateBackPackCell(itemInfo, itemInfoDict[itemInfo]);
    }

    /// <summary>
    /// 查询是否已拥有该道具
    /// </summary>
    /// <param name="idArray"></param>
    /// <returns></returns>
    public bool Exist(params int[] idArray)
    {
        for (int i = 0; i < idArray.Length; i++)
        {
            int index = itemInfoLsit.FindIndex(itemInfo => { return itemInfo.ID == idArray[i]; });
            if (index != -1) { return true; }
        }
        return false;
    }
    public bool Exist(Item item)
    {
        return itemInfoDict.ContainsKey(item.itemInformation);
    }
}
