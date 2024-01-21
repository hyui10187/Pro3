using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour {
    
    private Dictionary<int, string[]> talkData;

    private void Awake() {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData() {
        
        // 사물 대사
        talkData.Add(100, new string[] { "영롱한 촛불이다." });
        talkData.Add(200, new string[] { "따뜻한 모닥불 앞에 있었더니 체력이 회복되었다." });
        talkData.Add(300, new string[] { "고풍스러운 괘종시계이다. 현재 시간은..." });
        talkData.Add(400, new string[] { "평범한 나무상자다." });
        talkData.Add(500, new string[] { "게시판에 안내사항이 써있다." });
        talkData.Add(600, new string[] { "어려운 책들이 꽂혀있다." });
        talkData.Add(700, new string[] { "창문 밖에 눈보라가 휘몰아치고 있다." });
        talkData.Add(800, new string[] { "잘 자라고 있는 식물이다." });
        talkData.Add(900, new string[] { "단단한 울타리이다." });
        talkData.Add(1000, new string[] { "아늑해 보이는 소파이다." });

        // NPC 대사
        talkData.Add(10000, new string[] { "안녕!", "목을 축일것 좀 줄까?", "편하게 쉬다 가렴" });
        talkData.Add(10000 + 10, new string[] { "어서와", "이 마을이 아직 낯설지?", "루나가 걱정이 있는것 같던데", "무슨 일인지 물어봐줄래?" });

        talkData.Add(20000, new string[] { "모닥불이 따뜻해서 너무 좋아", "밖은 너무 추워", "눈보라가 점점 심해지는것 같아" });
        talkData.Add(20000 + 11, new string[] { "어떡하면 좋지?", "동전을 잃어버렸는데 못찾겠어", "혹시 너가 찾아줄 수 있니?" });
        
        talkData.Add(10000 + 20, new string[] { "루나가 동전을 잃어버렸구나", "얼른 찾았으면 좋겠다" });
        talkData.Add(20000 + 20, new string[] { "찾으면 꼭 좀 가져다 줘" });
        talkData.Add(30000 + 20, new string[] { "근처에서 동전을 찾았다" });
        
        talkData.Add(20000 + 21, new string[] { "정말 고마워", "이건 내 작은 보답이야" });
    }

    public string GetTalk(int sumId, int talkIndex) {

        Debug.Log("TalkManager의 GetTalk 진입..........................");
        
        if(!talkData.ContainsKey(sumId)) {
            if(!talkData.ContainsKey(sumId - sumId % 10)) {
                // 퀘스트 맨 처음 대사마저 없을때
                // 기본 대사를 가지고 온다
                if(talkIndex == talkData[sumId - sumId % 100].Length)
                    return null;
                else
                    return talkData[sumId - sumId % 100][talkIndex];
                
            } else {
                // 해당 퀘스트 진행순서 대사가 없을때
                // 퀘스트 맨 처음 대사를 가지고 온다
                if(talkIndex == talkData[sumId - sumId % 10].Length)
                    return null;
                else
                    return talkData[sumId - sumId % 10][talkIndex];
            }
        }

        if(talkIndex == talkData[sumId].Length)
            return null;
        else
            return talkData[sumId][talkIndex];
    }
    
}