using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int Gold = 0;
    public List<InventoryItem> Items = new List<InventoryItem>();

    public bool PickUpItem(GameObject obj)
    {
        switch(obj.tag)
        {
            case Constants.TAG_CURRENCY:
                return true;
            case Constants.TAG_POTION:
                Potion Potion = obj.GetComponent<Potion>();
                Items.Add(new InventoryItem($"{Potion.Color} Potion", Potion.Description, Potion.Actions));
                return true;
            default:
                return false;
        }
    }
}
