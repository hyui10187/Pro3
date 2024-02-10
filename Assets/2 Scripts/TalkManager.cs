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
        
        // 사물 대사   // 사물 대사는 100 단위   // 퀘스트 아이템은 1000 단위
        talkData.Add(100, new string[] { "영롱한 촛불이다." });
        talkData.Add(200, new string[] { "따뜻한 모닥불 앞에 있었더니 체력이 회복되었다." });
        talkData.Add(300, new string[] { "고풍스러운 괘종시계이다. 현재 시간은..." });
        talkData.Add(400, new string[] { "평범한 나무상자다." });
        talkData.Add(500, new string[] { "게시판에 안내사항이 써있다." });
        talkData.Add(600, new string[] { "어려운 책들이 꽂혀있다." });
        talkData.Add(700, new string[] { "창문 밖에 눈보라가 휘몰아치고 있다." });
        talkData.Add(800, new string[] { "잘 자라고 있는 식물이다." });
        talkData.Add(900, new string[] { "단단한 울타리이다." });
        talkData.Add(1100, new string[] { "아늑해 보이는 소파이다." });
        talkData.Add(1200, new string[] { "튼튼해 보이는 나무이다.", "도끼가 있어야 제거할 수 있을것 같다." });
        talkData.Add(1300, new string[] { "럼주가 잘 익어가고 있다." });
        talkData.Add(1400, new string[] { "원목으로 만든 테이블이다." });
        talkData.Add(1500, new string[] { "금화가 가득 담긴 꾸러미다." });
        talkData.Add(1600, new string[] { "세월의 흔적이 느껴지는 탁자다." });
        talkData.Add(1700, new string[] { "세계지도가 걸려있다." });
        talkData.Add(1800, new string[] { "딱딱한 나무 의자다." });
        talkData.Add(1900, new string[] { "여러명이 앉을 수 있는 의자다." });
        talkData.Add(2100, new string[] { "모험에 관한 흥미로운 책들이 꽂혀있다." });
        talkData.Add(2200, new string[] { "여권에 투숙하는 사람들의 명단이 적혀있다." });
        talkData.Add(2300, new string[] { "항아리에 귀한 향신료가 담겨있다." });

        // NPC 대사   // NPC 대사는 10000 단위
        talkData.Add(10000, new string[] { "안녕!", "목을 축일것 좀 줄까?", "편하게 쉬다 가렴" });
        talkData.Add(20000, new string[] { "모닥불이 따뜻해서 너무 좋아", "밖은 너무 추워", "눈보라가 점점 심해지는것 같아" });
        talkData.Add(30000, new string[] { "강인한 용사가 되고 싶나?", "실력으로 증명해봐" });
        talkData.Add(40000, new string[] { "이 마을 왼쪽에는 동굴이 있어", "예전에는 그 동굴을 통해 다른 지역으로 갈 수 있었지", "근데 지금은 나무가 동굴 입구를 막고 있어", "누가 나무를 베어주면 정말 좋을텐데" });
        talkData.Add(50000, new string[] { "저 아래에는 위험한 몬스터들이 살고 있어", "그래서 아무나 들어갈 수 없도록 막아놓은거야" });
        talkData.Add(60000, new string[] { "여어", "잘 지내지?", "또 보자구" });
        talkData.Add(70000, new string[] { "저 넓은 세상이 보여?", "나는 언젠가 모든 곳을 여행할꺼야", "그 날을 위해 지금은 준비중이야" });
        talkData.Add(80000, new string[] { "어서와", "피곤하지?", "여관에서 하루밤 묵고 갈래?" });
        talkData.Add(90000, new string[] { "너무 배고파", "다 먹어버릴거야" });
        
        talkData.Add(10000 + 10, new string[] { "어서와", "이 마을이 아직 낯설지?", "루나가 걱정이 있는것 같던데", "무슨 일인지 물어봐줄래?" });
        talkData.Add(20000 + 10 + 1, new string[] { "어떡하면 좋지?", "동전을 잃어버렸는데 못찾겠어", "혹시 너가 찾아줄 수 있니?" });

        talkData.Add(1000 + 20, new string[] { "근처에서 동전을 찾았다" });
        talkData.Add(10000 + 20, new string[] { "루나가 동전을 잃어버렸구나", "얼른 찾았으면 좋겠다" });
        talkData.Add(20000 + 20, new string[] { "찾으면 꼭 좀 가져다 줘" });
        talkData.Add(20000 + 20 + 1, new string[] { "정말 고마워", "이건 내 작은 보답이야" });
        
        talkData.Add(10000 + 30, new string[] { "루나의 고민이 해결 되어서 다행이야", "그런데 이 마을에는 커다란 문제가 있어", "얼마전부터 몬스터들이 나타나서 사람들을 위혐하고 있거든", "밖에 있는 몬스터들을 좀 처치해 줄 수 있을까?" });
        talkData.Add(20000 + 30 + 1, new string[] { "우리 마을을 구해줘", "너만 믿을께!" });
        
        talkData.Add(10000 + 40, new string[] { "마을 사람들이 불안에 떨고 있어", "몬스터들을 꼭 처치해줘" });
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