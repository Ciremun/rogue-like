using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip soundEffect;
    private bool colliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliding) return;
        colliding = true;
        Inventory Inventory = collision.GetComponent<Inventory>();
        if (Inventory && Inventory.PickUpItem(gameObject))
        {
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        Destroy(gameObject);
    }
}
