using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inventory = collision.GetComponent<Inventory>();
        if (inventory && inventory.PickUpItem(gameObject))
        {
            Destroy(gameObject);
        }
    }
}
