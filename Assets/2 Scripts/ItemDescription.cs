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
        descriptionData.Add("초록물약", "사용시 HP를 100 감소시킴");
        descriptionData.Add("노랑물약", "사용시 이동속도를 0.2% 증가시킴");
        descriptionData.Add("보라물약", "사용시 HP를 100 감소시킴");
        descriptionData.Add("사탕", "써니가 좋아할것 같은 달콤한 사탕이다.");
        descriptionData.Add("소드", "장인의 손길이 느껴지는 검이다.\n착용시 공격력을 20 증가시켜준다.");
        descriptionData.Add("반지", "아름다운 보석이 박혀있는 반지다.\n착용시 MP 최대치를 10 증가시켜준다.");
        descriptionData.Add("목걸이", "귀한 재료로 만든 목걸이다.");
        descriptionData.Add("은화", "루나가 잃어버린 은화이다.");
        descriptionData.Add("상점 열쇠", "상점으로 가는 문을 열 수 있는 열쇠다.");
        descriptionData.Add("상자 열쇠", "잠겨 있는 상자를 열 수 있는 열쇠다.");
        descriptionData.Add("목재", "나무로 된 제품을 만들 수 있는 재료다.");
        descriptionData.Add("돌멩이", "돌로 된 제품을 만들 수 있는 재료다.");
        descriptionData.Add("가죽갑옷", "가죽으로 만든 갑옷이다.");
        descriptionData.Add("가죽장갑", "가죽으로 만든 장갑이다.");
        descriptionData.Add("가죽모자", "가죽으로 만든 모자다.");
        descriptionData.Add("가죽신발", "가죽으로 만든 신발이다.");
        descriptionData.Add("나무방패", "나무로 만든 방패다.");
    }
    
    public string GetDescription(String itemName) {
        string itemDescription = descriptionData[itemName];
        
        return itemDescription;
    }
    
}