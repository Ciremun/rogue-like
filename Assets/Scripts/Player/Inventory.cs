using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

    public int space = 25;
    public int gold = 0;
    public List<InventoryItem> items = new List<InventoryItem>();

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Inventory instance != null");
            return;
        }

        instance = this;
    }

    public bool Add(GameObject obj)
    {
        if (items.Count >= space)
        {
            Debug.LogWarning("Inventory Full");
            return false;
        }
        switch(obj.tag)
        {
            case Constants.TAG_CURRENCY:
                Currency currency = obj.GetComponent<Currency>();
                gold += currency.amount;
                break;
            case Constants.TAG_POTION:
                Potion potion = obj.GetComponent<Potion>();
                items.Add(new InventoryItem($"{potion.color} Potion", potion.description, potion.actions, potion.icon));
                break;
            default:
                return false;
        }
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public void Remove(InventoryItem item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
