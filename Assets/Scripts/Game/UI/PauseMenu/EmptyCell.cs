using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包MVC模式--View层。空背包格子
/// </summary>
public class EmptyCell : MonoBehaviour
{
    [HideInInspector]
    public bool isEmpty = true;
    [HideInInspector]
    public ItemCell itemCell;
}
