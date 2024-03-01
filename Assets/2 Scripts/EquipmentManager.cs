using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

    public static EquipmentManager instance;

    public GameObject equipmentSlotsParent;
    public EquipmentSlot[] equipmentSlots;
    
    private void Awake() {
        instance = this;
        Init();
    }

    private void Init() {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }
    
}