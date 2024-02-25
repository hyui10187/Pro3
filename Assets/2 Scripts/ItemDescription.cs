using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription : MonoBehaviour {

    public static ItemDescription instance;
    
    private Dictionary<string, string> descriptionData;
    
    private void Awake() {
        descriptionData = new Dictionary<string, string>();
        GenerateData();
        instance = this;
    }

    private void GenerateData() {
        descriptionData.Add("빨강물약", "사용시 HP를 10 회복시킴");
        descriptionData.Add("파랑물약", "사용시 MP를 10 회복시킴");
        descriptionData.Add("초록물약", "사용시 HP를 -100 감소시킴");
        descriptionData.Add("노랑물약", "사용시 이동속도를 0.2% 증가시킴");
        descriptionData.Add("보라물약", "사용시 HP를 -100 감소시킴");
    }
    
    public string GetDescription(String itemName) {
        string itemDescription = descriptionData[itemName];
        
        return itemDescription;
    }
    
}