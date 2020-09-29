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
    float activeSceond;

    private void Awake()
    {
        cellList = new List<BackpackCell>();
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        activeSceond = pausePanel.activeSceond;
        IsShowToolTip = false;
    }

    private void Update()
    {

        if (IsShowToolTip)
        {
            MoveToolTip();
        }
    }

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

}
