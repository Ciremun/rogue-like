using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public string Color = "Purple";
    public string Description = "Potion Item";
    public List<InventoryItemAction> Actions = new List<InventoryItemAction>
    {
        new InventoryItemAction(Constants.ACTION_DROP),
        new InventoryItemAction(Constants.ACTION_THROW)
    };
}
