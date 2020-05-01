using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButtonEvent : EventTrigger
{
     Color normalColor = Color.white;
     Color highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1);

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        transform.GetComponent<Image>().color = highlightedColor;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        transform.GetComponent<Image>().color = normalColor;
    }
}
