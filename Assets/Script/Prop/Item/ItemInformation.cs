using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[HideInInspector]
public struct ItemInformation
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Attributes { get; set; }
    public string Description { get; set; }
    public Sprite Sprite { get; set; }

    public ItemInformation(int id, string name, string attributes, string description, Sprite sprite)
    {
        this.ID = id;
        this.Name = name;
        this.Attributes = attributes;
        this.Description = description;
        this.Sprite = sprite;
    }
}
