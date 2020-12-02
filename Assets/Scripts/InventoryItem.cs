using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public Sprite icon;
    public string name;
    public string description;
    public List<GameObject> actions;
    public GameObject itemObj;

    public InventoryItem(GameObject obj, string newName, string newDesc, List<GameObject> newActions, Sprite newIcon)
    {
        itemObj = obj;
        name = newName;
        description = newDesc;
        actions = newActions;
        icon = newIcon;
    }
}
