using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CommonUnit
{
    public static bool ComponentCheck(GameObject gameObject, params Type[] array)
    {
        foreach (var type in array)
        {
            Component component = gameObject.GetComponent(type);
            if (component != null)
            {
                return true;
            }
        }
        return false;
    }
    public static bool ComponentCheck(GameObject gameObject, List<Type> list)
    {
        foreach (var type in list)
        {
            Component component = gameObject.GetComponent(type);
            if (component != null)
            {
                return true;
            }
        }
        return false;
    }

    public static bool TagCheck(GameObject gameObject, params string[] array)
    {
        foreach (var item in array)
        {
            if (gameObject.tag == item)
            {
                return true;
            }
        }
        return false;
    }
    public static bool TagCheck(GameObject gameObject, List<string> list)
    {
        foreach (var item in list)
        {
            if (gameObject.tag == item)
            {
                return true;
            }
        }
        return false;
    }

    public static bool LayerCheck(GameObject gameObject, List<string> list)
    {
        foreach (var item in list)
        {
            if (gameObject.layer == GameDate.layer[item])
            {
                return true;
            }
        }
        return false;
    }
    public static bool LayerCheck(GameObject gameObject, params string[] array)
    {
        foreach (var item in array)
        {
            if (gameObject.layer == GameDate.layer[item])
            {
                return true;
            }
        }
        return false;
    }
}
