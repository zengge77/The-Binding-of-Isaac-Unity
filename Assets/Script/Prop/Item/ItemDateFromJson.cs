using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ItemDateFromJson
{
    private static List<ItemInformation> itemInfoList;

    public static ItemInformation GetItemInformation(int index)
    {
        if (itemInfoList == null)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "ItemDate.json");
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                itemInfoList = JsonConvert.DeserializeObject<List<ItemInformation>>(dataAsJson);
            }
        }
        ItemInformation go = itemInfoList.Find(a => a.ID == index);
        return go;
    }
}
