using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour {
    
    public Item item;

    public void GroceryStoreSlotClick() {
        PanelManager.instance.PurchaseAmountPanelOn(item);
    }

    public void EquipmentStoreSlotClick() {
        PanelManager.instance.PurchaseConfirmPanelOn(item);
    }
    
}