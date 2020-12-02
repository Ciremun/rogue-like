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

    public void Execute(GameObject dialog)
    {
        InventorySlotDialog dialogObject = dialog.GetComponent<InventorySlotDialog>();
        GameObject player = GameObject.FindWithTag("Player");
        switch(actionName)
        {
            case Constants.ACTION_DROP:
                Instantiate(dialogObject.dialogItem.itemObj, new Vector3(player.transform.position.x + 1, player.transform.position.y, 0), Quaternion.identity);
                Inventory playerInventory = player.GetComponent<Inventory>();
                playerInventory.Remove(dialogObject.dialogItem);
                return;
            case Constants.ACTION_THROW:
                return;
        }
    }
}
