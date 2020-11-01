using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip soundEffect;
    private bool destroyed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed) return;
        Inventory Inventory = collision.GetComponent<Inventory>();
        if (Inventory && Inventory.Add(gameObject))
        {
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        Destroy(gameObject);
        destroyed = true;
    }
}
