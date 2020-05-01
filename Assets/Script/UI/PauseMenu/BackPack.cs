using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPack : MonoBehaviour
{
    public PausePanel pausePanel;
    public Transform node;
    public ToolTip toolTip;
    public BackpackCell cellPrefab;
    List<BackpackCell> cellList;

    Animator myAnimator;
    CanvasGroup canvasGroup;
    bool IsShowToolTip;

    private void Awake()
    {
        cellList = new List<BackpackCell>();
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        IsShowToolTip = false;
    }

    public void SetActivation(bool value)
    {
        if (value)
        {
            myAnimator.Play("Enter");
            Invoke("DelayActivation", pausePanel.ActiveSceond);
        }

        if (!value)
        {
            myAnimator.Play("Exit");
            canvasGroup.interactable = false;
            HideToolTip();
        }
    }

    public void AddBackpackCell(ItemInformation itemInfo)
    {
        var cell = Instantiate(cellPrefab, node);
        cell.Initialization(this, itemInfo);
        cellList.Add(cell);
    }

    public void ShowToolTip(ItemInformation itemInfo)
    {
        toolTip.Show(itemInfo);
        IsShowToolTip = true;
    }
    public void HideToolTip()
    {
        toolTip.Hide();
        IsShowToolTip = false;
    }
    public void MoveToolTip()
    {
        toolTip.Move(Input.mousePosition);
    }

    void DelayActivation()
    {
        canvasGroup.interactable = true;
    }

    private void Update()
    {

        if (IsShowToolTip)
        {
            MoveToolTip();
        }
    }

}
