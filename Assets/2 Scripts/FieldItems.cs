using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour {

    public Item item;
    public SpriteRenderer image;

    public Item GetItem() {
        return item;
    }
    
    public void SetItem(Item paramItem) {
        item.itemType = paramItem.itemType;
        item.itemName = paramItem.itemName;
        item.itemImage = paramItem.itemImage;
        item.itemEffects = paramItem.itemEffects;

        image.sprite = item.itemImage;
    }

    public void DestroyItem() {
        gameObject.SetActive(false);
    }
    
}