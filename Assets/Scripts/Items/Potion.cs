using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public Sprite icon;
    public string color = "Purple";
    public string description = "Potion Item";
    public List<GameObject> actions;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        icon = spriteRenderer.sprite;
    }
}
