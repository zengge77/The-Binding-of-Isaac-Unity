using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 背包MVC模式--View层。物品格子，实现多个鼠标事件接口，与Control层相互通讯更新
/// </summary>
public class ItemCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public ItemInformation itemInfo;
    private int itemCount;
    private Text text;
    private Image image;

    [HideInInspector]
    public EmptyCell emptyCell;
    private BackPack backpack;

    private bool enableCheckDistance = false;
    private bool enableDraw = false;
    private Vector2 beginDragPoint;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    public void Initialization(BackPack backpack, EmptyCell emptyCell, ItemInformation itemInfo)
    {
        this.itemInfo = itemInfo;
        this.backpack = backpack;
        this.emptyCell = emptyCell;
        image.sprite = itemInfo.Sprite;
        gameObject.name = itemInfo.Name;
        UpdateItemCount(1);
    }

    public void UpdateItemCount(int count)
    {
        itemCount = count;
        if (count == 1)
        {
            text.text = String.Empty;
        }
        else
        {
            text.text = count.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backpack.ShowToolTip(itemInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backpack.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        enableCheckDistance = true;
        beginDragPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (enableDraw)
        {
            transform.position = eventData.position;
        }
        if (enableCheckDistance && (eventData.position - beginDragPoint).magnitude >= 10)
        {
            enableDraw = true;
            enableCheckDistance = false;
            //开始拖拽后即使raycastTarget关闭，该次的整个OnDrag及EndDrag也能正常执行
            image.raycastTarget = false;
            //拖拽时因为各个ItemCell生成顺序的先后,导致渲染顺序不同，在这里统一设置为Item Area的子物体，与Grid Content同级；
            transform.SetParent(transform.parent.parent.parent);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        enableDraw = false;
        enableCheckDistance = false;
        image.raycastTarget = true;

        EmptyCell newEmptycCell = UIUnit.GetFirstPickUI<EmptyCell>(eventData.position);
        backpack.SwapBackPackCell(this, newEmptycCell);
    }
}
