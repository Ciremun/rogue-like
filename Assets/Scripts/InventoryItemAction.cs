using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemAction
{
    public string name;

    public InventoryItemAction(string newName)
    {
        name = newName;
    }

    public void Execute(GameObject obj)
    {
        switch(name)
        {
            case Constants.ACTION_DROP:
                return;
            case Constants.ACTION_THROW:
                return;
        }
    }
}
