using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    public Item item;
    public SpriteRenderer image;

    public Item GetItem()
    {
        return item;
    }
    
    public void SetItem(Item paramItem)
    {
        item.itemType = paramItem.itemType;
        item.itemName = paramItem.itemName;
        item.itemImage = paramItem.itemImage;
        item.itemCount = paramItem.itemCount;
        item.itemPrice = paramItem.itemPrice;
        item.itemEffects = paramItem.itemEffects;

        image.sprite = item.itemImage;
    }

    public void DestroyItem()
    {
        gameObject.SetActive(false);
    }
    
}