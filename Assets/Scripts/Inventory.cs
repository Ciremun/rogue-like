using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();

    public bool PickUpItem(GameObject obj)
    {
        switch(obj.tag)
        {
            case "Currency":
                return true;
            case "Item":
                inventory.Add(obj);
                return true;
            default:
                return false;
        }
    }
}
