using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public Sprite icon;
    public string name;
    public string description;
    public List<InventoryItemAction> actions;

    public InventoryItem(string newName, string newDesc, List<InventoryItemAction> newActions, Sprite newIcon)
    {
        name = newName;
        description = newDesc;
        actions = newActions;
        icon = newIcon;
    }
}
