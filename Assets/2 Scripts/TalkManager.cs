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
        
        // talkData.Add(102, new string[] { "평범한 나무상자다." });
        // talkData.Add(103, new string[] { "게시판에 안내사항이 써있다." });
        // talkData.Add(104, new string[] { "어려운 책들이 꽂혀있다." });
        // talkData.Add(105, new string[] { "창문 밖에 눈보라가 휘몰아치고 있다." });
        // talkData.Add(106, new string[] { "잘 자라고 있는 식물이다." });
        // talkData.Add(107, new string[] { "단단한 울타리이다." });
        // talkData.Add(108, new string[] { "아늑해 보이는 소파이다." });
        
        // 사물 대사
        talkData.Add(3000, new string[] { "영롱한 촛불이다." });
        talkData.Add(4000, new string[] { "따뜻한 모닥불 앞에 있었더니 체력이 회복되었다." });
        
        // NPC 대사
        talkData.Add(1000, new string[] { "안녕?", "이곳에 처음 왔구나?", "편하게 쉬다 가렴" });
        talkData.Add(1000 + 10, new string[] { "어서와", "이 마을에 놀라운 전설이 있다는데", "오른쪽 호수에 루도가 알려줄꺼야" });

        talkData.Add(2000, new string[] { "처음보는 얼굴이네?", "밖은 너무 추워", "눈보라가 점점 심해지는것 같아" });
        talkData.Add(2000 + 11, new string[] { "여어", "이 호수의 전설을 들으러 온거야?", "그럼 일좀 하나 해주면 좋을텐데", "근처에 떨어진 동전을 주워줘" });
        
        talkData.Add(1000 + 20, new string[] { "루도의 동전?", "돈을 흘리고 다니면 못쓰지", "나중에 루도에게 한마디 해야겠어" });
        talkData.Add(2000 + 20, new string[] { "찾으면 꼭 좀 가져다 줘" });
        talkData.Add(5000 + 20, new string[] { "근처에서 동전을 찾았다" });
        
        talkData.Add(2000 + 21, new string[] { "엇 찾아줘서 고마워" });
    }

    public string GetTalk(int sumId, int talkIndex) {

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