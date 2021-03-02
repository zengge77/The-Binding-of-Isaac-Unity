using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButtonEvent : EventTrigger
{
    Image image;

    Color normalColor = Color.white;
    Color highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1);

    private void Awake()
    {
        image = transform.GetComponent<Image>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        image.color = highlightedColor;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        image.color = normalColor;
    }
}
