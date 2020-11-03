using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class InventorySlotDialog : MonoBehaviour
{
    public GameObject dialog;
    public GameObject itemNameObj;
    public GameObject itemDescriptionObj;
    public List<GameObject> itemActions;
    public Image icon;

    private Text itemName;
    private Text itemDescription;

    void Start()
    {
        itemName = itemNameObj.GetComponent<Text>();
        itemDescription = itemDescriptionObj.GetComponent<Text>();
        dialog.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Escape") && dialog.activeSelf)
        {
            dialog.SetActive(false);
        }
    }

    public void UpdateDialog(InventoryItem item)
    {
        itemName.text = item.name;
        itemDescription.text = item.description;
        icon.sprite = item.icon;
        int imgwidth = (int)icon.sprite.rect.width;
        int imgheight = (int)icon.sprite.rect.height;
        Resize.fit(ref imgwidth, ref imgheight, 30, 30);
        icon.rectTransform.sizeDelta = new Vector2(imgwidth, imgheight);
        icon.enabled = true;
    }
}
