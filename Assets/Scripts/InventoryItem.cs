using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public string Name;
    public string Description;
    public List<InventoryItemAction> Actions;

    public InventoryItem(string name, string description, List<InventoryItemAction> actions)
    {
        Name = name;
        Description = description;
        Actions = actions;
    }
}
