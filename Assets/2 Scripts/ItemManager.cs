using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour {

    public static ItemManager instance;
    
    public List<Item> itemDB;
    public GameObject fieldItemPrefab;
    public Transform[] itemSpawnPos;
    
    private void Awake() {
        instance = this;
    }

    private void Start() {
        
        for(int i = 0; i < 5; i++) {
            GameObject obj = Instantiate(fieldItemPrefab, itemSpawnPos[i].position, Quaternion.identity);
            FieldItems fieldItems = obj.GetComponent<FieldItems>();
            fieldItems.SetItem(itemDB[Random.Range(0, 3)]); // 0, 1, 2 중에서 무작위로 설정해줌
        }
    }
    
}