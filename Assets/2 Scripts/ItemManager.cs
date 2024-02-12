using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour {

    public static ItemManager instance;
    
    public List<Item> itemDB;
    public List<Item> fruitDB;
    public List<Item> woodDB;
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
        obj.transform.localScale *= 3f; // FieldItem Prefab의 크기가 작으니 그거의 3배를 곱해줘서 적절한 크기로 생성하기
        FieldItems fieldItems = obj.GetComponent<FieldItems>();
        fieldItems.SetItem(fruitDB[Random.Range(0, 3)]); // 0, 1, 2 열매 중에서 무작위로 드랍하도록
    }
    
    public void DropWood(Vector3 dropPos) {
        GameObject obj = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity, fieldItemParent.transform);
        obj.transform.localScale *= 3f;
        FieldItems fieldItems = obj.GetComponent<FieldItems>();
        fieldItems.SetItem(woodDB[0]); // 0, 1 나무 중에서 무작위로 드랍하도록
    }
    
}