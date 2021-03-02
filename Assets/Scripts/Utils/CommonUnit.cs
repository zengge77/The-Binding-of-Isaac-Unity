using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CommonUnit
{
    public static void SelectNewDestination(Collision2D collision)
    {
        //ContactPoint2D contacts = collision.contacts[0];
        //Vector2 direction = (contacts.point - (Vector2)transform.position).normalized;

        //int angle = UnityEngine.Random.Range(90, 270);
        //Vector2 newDirection = Quaternion.AngleAxis(angle, Vector3.forward) * direction;

        //int multiple = UnityEngine.Random.Range(2, 5);
        //Vector2 point = newDirection * multiple;
        //destination = (Vector2)transform.position - point;
    }

    public static bool IsItOfThisType(List<Type> list, GameObject gameObject)
    {
        foreach (var type in list)
        {
            Component comp = gameObject.GetComponent(type);
            if (comp != null)
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsItOfThisType(List<string> list, GameObject gameObject)
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
}
