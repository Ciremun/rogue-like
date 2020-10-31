using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemAction
{
    public string Name;

    public InventoryItemAction(string name)
    {
        Name = name;
    }

    public void Execute(GameObject obj)
    {
        switch(Name)
        {
            case Constants.ACTION_DROP:
                return;
            case Constants.ACTION_THROW:
                return;
        }
    }
}
