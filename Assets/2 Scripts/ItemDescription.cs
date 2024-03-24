using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription : MonoBehaviour {

    public static ItemDescription instance;
    
    private Dictionary<ItemName, string> descriptionData;
    
    private void Awake() {
        descriptionData = new Dictionary<ItemName, string>();
        GenerateData();
        instance = this;
    }

    private void GenerateData() {
        descriptionData.Add(ItemName.빨강물약, "사용시 HP를 10 회복시킴");
        descriptionData.Add(ItemName.파랑물약, "사용시 MP를 10 회복시킴");
        descriptionData.Add(ItemName.초록물약, "사용시 즉시 사망함");
        descriptionData.Add(ItemName.노랑물약, "사용시 이동속도를 0.2% 증가시킴");
        descriptionData.Add(ItemName.보라물약, "사용시 즉시 사망함"); // 나중에 공격속도 물약으로 변경해주기
        descriptionData.Add(ItemName.사탕, "써니가 좋아할것 같은 달콤한 사탕이다.");
        descriptionData.Add(ItemName.소드, "장인의 손길이 느껴지는 검이다.\n착용시 공격력을 20 증가시켜준다.");
        descriptionData.Add(ItemName.활, "단단한 나무로 만든 활이다.\n착용시 공격력을 20 증가시켜준다.");
        descriptionData.Add(ItemName.화살, "단단한 나무로 만든 화살이다.\n활이 있어야 사용 가능하다.");
        descriptionData.Add(ItemName.반지, "아름다운 보석이 박혀있는 반지다.\n착용시 MP를 10 증가시켜준다.");
        descriptionData.Add(ItemName.목걸이, "귀한 재료로 만든 목걸이다.\n착용시 민첩을 1 증가시켜준다.");
        descriptionData.Add(ItemName.은화, "루나가 잃어버린 은화이다.");
        descriptionData.Add(ItemName.상점열쇠, "상점으로 가는 문을 열 수 있는 열쇠다.");
        descriptionData.Add(ItemName.상자열쇠, "잠겨 있는 상자를 열 수 있는 열쇠다.");
        descriptionData.Add(ItemName.목재, "나무로 된 제품을 만들 수 있는 재료다.");
        descriptionData.Add(ItemName.돌멩이, "돌로 된 제품을 만들 수 있는 재료다.");
        descriptionData.Add(ItemName.가죽갑옷, "가죽으로 만든 갑옷이다.");
        descriptionData.Add(ItemName.가죽장갑, "가죽으로 만든 장갑이다.");
        descriptionData.Add(ItemName.가죽모자, "가죽으로 만든 모자다.");
        descriptionData.Add(ItemName.가죽신발, "가죽으로 만든 신발이다.");
        descriptionData.Add(ItemName.나무방패, "나무로 만든 방패다.");
        descriptionData.Add(ItemName.붉은과일, "사용시 HP를 10 회복시킴");
        descriptionData.Add(ItemName.푸른과일, "사용시 MP를 10 회복시킴");
        descriptionData.Add(ItemName.보라과일, "사용시 즉시 사망함");
    }
    
    public string GetDescription(ItemName itemName) {
        string itemDescription = descriptionData[itemName];
        return itemDescription;
    }
    
}