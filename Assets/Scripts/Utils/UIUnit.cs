using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIUnit
{
    public static T GetFirstPickUI<T>(Vector2 position) where T : MonoBehaviour
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        //射线检测ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        T ui = null;
        for (int i = 0; i < uiRaycastResultCache.Count; i++)
        {
            ui = uiRaycastResultCache[i].gameObject.GetComponent<T>();
            if (ui != null) { return ui; }
        }
        return null;
    }
}
