using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    BackPack backpack;
    ItemInformation itemInfo;

    public void Initialization(BackPack backpack, ItemInformation itemInfo)
    {
        this.backpack = backpack;
        this.itemInfo = itemInfo;
        GetComponent<Image>().sprite = itemInfo.Sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backpack.ShowToolTip(itemInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backpack.HideToolTip();
    }
}
