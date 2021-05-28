using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  背包MVC模式--Control层。接收来自Model层的数据并更新View层，接收View层的数据并更新Model层或View层
/// </summary>
public class BackPack : MonoBehaviour
{
    public PausePanel pausePanel;
    public ScrollRect scrollRect;
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform gridLayoutGroupRect;
    public ToolTip toolTip;
    private int cellCountPerRow = 4;


    public EmptyCell emptyCellPrefab;
    private List<EmptyCell> emptyCellList;
    public ItemCell itemCellPrefab;
    private List<ItemCell> itemCellList;

    private Animator myAnimator;
    private CanvasGroup canvasGroup;
    private bool isShowToolTip;
    private float activeSceond;

    private void Awake()
    {
        emptyCellList = new List<EmptyCell>();
        itemCellList = new List<ItemCell>();
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        activeSceond = pausePanel.activeSceond;
        isShowToolTip = false;
    }

    private void Start()
    {
        EmptyCell[] array = gridLayoutGroup.transform.GetComponentsInChildren<EmptyCell>();
        emptyCellList.AddRange(array);
        UpdateGridLayoutGroupRectSize();
    }

    private void Update()
    {
        if (isShowToolTip)
        {
            MoveToolTip();
        }
    }

    // EmptyCell和ItemCell相关操作
    public void UpdateBackPackCell(ItemInformation itemInfo, int count)
    {
        if (count == 0)//删除道具格子
        {

        }
        else if (count == 1)//添加道具格子
        {
            if (emptyCellList.Count <= itemCellList.Count)
            {
                for (int i = 0; i < cellCountPerRow; i++)
                {
                    emptyCellList.Add(Instantiate(emptyCellPrefab, gridLayoutGroup.transform));
                }
                UpdateGridLayoutGroupRectSize();
            }

            EmptyCell emptyCell = emptyCellList.Find(targetCell => targetCell.isEmpty);
            ItemCell itemCell = Instantiate(itemCellPrefab);
            itemCell.Initialization(this, emptyCell, itemInfo);
            BindingBackPackCell(emptyCell, itemCell);
            itemCellList.Add(itemCell);
        }
        else if (count > 1)//更新道具数量显示
        {
            ItemCell itemCell = itemCellList.Find(targetCell => targetCell.itemInfo.ID == itemInfo.ID);
            itemCell.UpdateItemCount(count);
        }
    }
    public void SwapBackPackCell(ItemCell orginalItemCell, EmptyCell newEmptyCell)
    {
        //不变
        if (newEmptyCell == null || newEmptyCell == orginalItemCell.emptyCell)
        {
            //orginalItemCell.transform.position = orginalItemCell.emptyCell.transform.position;
            BindingBackPackCell(orginalItemCell.emptyCell, orginalItemCell);
            return;
        }

        //交换
        EmptyCell orginalEmptyCell = orginalItemCell.emptyCell;
        ItemCell newItemCell = newEmptyCell.itemCell;
        BindingBackPackCell(newEmptyCell, orginalItemCell);
        BindingBackPackCell(orginalEmptyCell, newItemCell);
    }
    private void UpdateGridLayoutGroupRectSize()
    {
        int row = emptyCellList.Count / cellCountPerRow;//行数
        float height = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;//高度
        gridLayoutGroupRect.sizeDelta = new Vector2(gridLayoutGroupRect.sizeDelta.x, height * row);
        scrollRect.verticalNormalizedPosition = 1;
    }
    private void BindingBackPackCell(EmptyCell emptyCell, ItemCell itemCell)
    {
        if (itemCell != null)
        {
            emptyCell.isEmpty = false;
            emptyCell.itemCell = itemCell;
            itemCell.emptyCell = emptyCell;
            itemCell.transform.SetParent(emptyCell.transform);
            itemCell.transform.localPosition = Vector3.zero;
        }
        else
        {
            emptyCell.isEmpty = true;
            emptyCell.itemCell = null;
        }
    }

    // ToolTip相关操作
    public void ShowToolTip(ItemInformation itemInfo)
    {
        toolTip.Show(itemInfo);
        isShowToolTip = true;
    }
    public void HideToolTip()
    {
        toolTip.Hide();
        isShowToolTip = false;
    }
    public void MoveToolTip()
    {
        toolTip.Move(Input.mousePosition);
    }

    //自身激活
    public void SetActivation(bool value)
    {
        if (value)
        {
            myAnimator.Play("Enter");
            StartCoroutine(DelayActivation(activeSceond));
        }

        if (!value)
        {
            myAnimator.Play("Exit");
            canvasGroup.interactable = false;
            HideToolTip();
        }
    }
    IEnumerator DelayActivation(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        canvasGroup.interactable = true;
    }
}
