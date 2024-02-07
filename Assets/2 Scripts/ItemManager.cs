using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour {

    public static ItemManager instance;
    
    public List<Item> itemDB;
    public List<Item> fruitDB;
    public GameObject fieldItemPrefab;
    public Transform[] itemSpawnPos;
    public int itemSpawnNum; // 생성해줄 아이템의 갯수
    public GameObject fieldItemParent;
    
    private void Awake() {
        instance = this;
    }

    public void GenerateItem() {
        for(int i = 0; i < itemSpawnNum; i++) {
            GameObject obj = Instantiate(fieldItemPrefab, itemSpawnPos[i].position, Quaternion.identity, fieldItemParent.transform);
            FieldItems fieldItems = obj.GetComponent<FieldItems>();
            fieldItems.SetItem(itemDB[Random.Range(0, 5)]);
        }
    }

    public void DropItem(Vector3 dropPos) {
        GameObject obj = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity, fieldItemParent.transform);
        FieldItems fieldItems = obj.GetComponent<FieldItems>();
        fieldItems.SetItem(itemDB[Random.Range(0, 5)]);
    }

    public void DropFruit(Vector3 dropPos) {
        GameObject obj = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity, fieldItemParent.transform);
        //obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        obj.transform.localScale *= 3f;
        FieldItems fieldItems = obj.GetComponent<FieldItems>();
        fieldItems.SetItem(fruitDB[Random.Range(0, 2)]); // 0, 1 중에서 무작위로 설정해줌
    }
    
}