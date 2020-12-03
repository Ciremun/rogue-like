using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemAction : MonoBehaviour
{
    public string actionName;

    public InventoryItemAction(string newName)
    {
        actionName = newName;
    }

    public void Execute(GameObject dialogObj)
    {
        InventorySlotDialog dialog= dialogObj.GetComponent<InventorySlotDialog>();
        GameObject player = GameObject.FindWithTag("Player");
        switch(actionName)
        {
            case Constants.ACTION_DROP:
                InventoryItem item = dialog.slot.item;
                Inventory playerInventory = player.GetComponent<Inventory>();
                Instantiate(item.itemObj, new Vector3(player.transform.position.x + 1, player.transform.position.y, 0), Quaternion.identity);
                playerInventory.Remove(item);
                return;
            case Constants.ACTION_THROW:
                return;
        }
    }
}
