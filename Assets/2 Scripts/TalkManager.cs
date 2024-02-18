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
        talkData.Add(400, new string[] { "잠겨있는 나무상자다.", "열쇠가 있어야 열 수 있을것 같다." });
        talkData.Add(500, new string[] { "게시판에 안내사항이 써있다." });
        talkData.Add(600, new string[] { "어려운 책들이 꽂혀있다." });
        talkData.Add(700, new string[] { "창문 밖에 눈보라가 휘몰아치고 있다." });
        talkData.Add(800, new string[] { "잘 자라고 있는 식물이다." });
        talkData.Add(900, new string[] { "단단한 울타리다." });
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
        talkData.Add(2400, new string[] { "문이 굳게 잠겨있다.", "열쇠가 있어야 열 수 있을것 같다." });
        talkData.Add(2500, new string[] { "물이 상당히 깊어 보인다.", "배가 있어야 건널 수 있을것 같다." });
        talkData.Add(2600, new string[] { "작은 돌멩이다." });
        talkData.Add(2700, new string[] { "큰 나무의 밑동이다.", "부수면 목재를 많이 얻을 수 있을것 같다." });
        talkData.Add(2800, new string[] { "경고!!", "관계자 외에는 절대 아래층으로 내려가지 마시오!" });
        talkData.Add(2900, new string[] { "견고한 울타리가 막고 있다." });
        talkData.Add(3100, new string[] { "울타리가 부서져있다." });
        talkData.Add(3200, new string[] { "약간 무서워 보이는 석상이다." });
        talkData.Add(3300, new string[] { "오래된 해골이 있다.", "누구의 해골일까?" });
        talkData.Add(3400, new string[] { "집이 한채 있다.", "추위를 피할 수 있을것 같다." });
        talkData.Add(3500, new string[] { "작은 나무의 밑동이다.", "부수면 목재를 얻을 수 있을것 같다." });
        talkData.Add(3600, new string[] { "작은 열매 나무다.", "부수면 열매를 얻을 수 있을것 같다." });
        talkData.Add(3700, new string[] { "작은 덤불이다.", "부수면 과일을 얻을 수 있을것 같다." });
        talkData.Add(3800, new string[] { "얼어붙은 바위다.", "곡괭이가 있어야 부술 수 있을것 같다." });
        talkData.Add(3900, new string[] { "예쁜 꽃이다.", "이 추운 지역에 어떻게 꽃이 있는 것일까?" });
        talkData.Add(4100, new string[] { "꽁꽁 얼어붙은 언덕이다.", "미끄러워서 올라갈 수 없다." });
        talkData.Add(4200, new string[] { "거대한 동굴이 있다.", "입구가 나무로 막혀있어서 들어갈 수 없다." });
        talkData.Add(4300, new string[] { "울창한 숲이 펼쳐져있다.", "나무가 빽빽해서 더이상 나아갈 수 없다." });
        talkData.Add(4400, new string[] { "탐스러운 버섯이다." });
        talkData.Add(4500, new string[] { "반짝이는 식물이다." });
        talkData.Add(4600, new string[] { "육중한 문이 있다.", "잠겨있어서 열리지 않는다.", "이 문은 어디로 통하는 것일까?" });
        talkData.Add(4700, new string[] { "비석이 있다.", "고대 문자가 적혀 있어서 읽을 수 없다." });
        talkData.Add(4800, new string[] { "튼튼한 철창문이다.", "안이 어두워서 잘 보이지 않는다." });
        talkData.Add(4900, new string[] { "창고에 맡긴 물품에 대한 기록이 꼼꼼하게 되어있다." });
        talkData.Add(5100, new string[] { "고풍스러운 턴테이블이다.", "어떤 음악을 틀어볼까?" });
        talkData.Add(5200, new string[] { "잘 닦여진 거울이다.", "역시 내가 세상에서 제일 예쁘군" });
        talkData.Add(5300, new string[] { "은은한 무드등의 조명 덕분에 마음이 평온해진다." });
        talkData.Add(5400, new string[] { "푹신한 소파이다.", "가끔은 침대보다 이 소파에서 자고 싶다." });
        talkData.Add(5500, new string[] { "오늘은 이만 잠자리에 들까?" });
        talkData.Add(5600, new string[] { "열대 지역에서 자라는 귀한 식물이다." });
        talkData.Add(5700, new string[] { "원목으로 만든 의자다.", "앉아서 명상을 하기에 좋을것 같다." });

        // NPC 대사   // NPC 대사는 10000 단위   // 이동형 NPC는 30000 단위
        talkData.Add(10000, new string[] { "안녕!", "목을 축일것 좀 줄까?", "편하게 쉬다 가렴" });
        talkData.Add(20000, new string[] { "모닥불이 따뜻해서 너무 좋아", "밖은 너무 추워", "눈보라가 점점 심해지는것 같아" });
        talkData.Add(30000, new string[] { "강인한 용사가 되고 싶나?", "실력으로 증명해봐" });
        talkData.Add(40000, new string[] { "이 마을 왼쪽에는 동굴이 있어", "예전에는 그 동굴을 통해 다른 지역으로 갈 수 있었지", "근데 지금은 나무가 동굴 입구를 막고 있어", "누가 나무를 베어주면 정말 좋을텐데" });
        talkData.Add(50000, new string[] { "저 아래에는 위험한 몬스터들이 살고 있어", "그래서 아무나 들어갈 수 없도록 막아놓은거야" });
        talkData.Add(60000, new string[] { "여어", "잘 지내지?", "또 보자구" });
        talkData.Add(70000, new string[] { "저 넓은 세상이 보여?", "나는 언젠가 모든 곳을 여행할꺼야", "그 날을 위해 지금은 준비중이야" });
        talkData.Add(80000, new string[] { "어서와!", "피곤하지?", "여관에서 하루밤 묵고 갈래?" });
        talkData.Add(90000, new string[] { "너무 배고파", "다 먹어버릴거야" });
        talkData.Add(100000, new string[] { "저 방안에서 이상한 소리가 들려", "무슨 소리일까?" });
        talkData.Add(110000, new string[] { "모험자여 반갑네", "밖의 추위에 노출되면 금방 얼어죽을걸세", "어서 따뜻한 지하로 가서 몸을 녹이시게" });
        talkData.Add(120000, new string[] { "사탕먹고 싶다", "언니 혹시 사탕있어?", "없다고? 칫..." });
        talkData.Add(130000, new string[] { "작은 생쥐다.", "가까이 가면 병균이 옮을것 같다." });
        talkData.Add(140000, new string[] { "어디보자 캐비어와 송로 버섯은 적당히 남았고...", "저기 미안한데 방해하지 말아줄래?" });
        talkData.Add(150000, new string[] { "어렸을때는 저 아래 광산에서 친구들이랑 같이 놀았었는데", "이제는 출입하지 못하도록 막아버렸어", "어린 시절이 그립네..." });
        talkData.Add(160000, new string[] { "좋은 하루야!", "가방이 무거우면 짐을 좀 맡아줄까?" });
        
        // 퀘스트 10: 카밀과 대화하기
        talkData.Add(10000 + 10, new string[] { "반가워!", "이 마을이 아직 낯설지?", "모두 좋은 사람들이라 금방 친해질거야", "벽난로 옆에 있는 루나랑 인사했어?", "루나가 걱정이 있는것 같던데", "무슨 일인지 물어봐줄래?" });
        
        // 퀘스트 20: 루나와 대화하기
        talkData.Add(20000 + 20, new string[] { "어떡하면 좋지?", "동전을 잃어버렸는데 못찾겠어", "혹시 너가 찾아줄 수 있니?", "석상이 있는 방에 떨어뜨린것 같아" });
        talkData.Add(10000 + 20 + 1, new string[] { "루나가 동전을 잃어버렸구나", "얼른 찾았으면 좋겠다" });

        // 퀘스트 30: 루나의 동전 찾아주기
        talkData.Add(1000 + 30, new string[] { "루나가 잃어버린 동전을 찾았다!" });
        talkData.Add(20000 + 30, new string[] { "찾으면 꼭 좀 가져다 줘", "석상이 있는 방에서 잃어버린것 같아" });
        talkData.Add(20000 + 30 + 1, new string[] { "동전을 찾아줘서 정말 고마워!", "감사의 의미로 이 반지를 줄께", "내 작은 보답이니까 사양하지 말아줘" });
        
        // 퀘스트 40: 촌장의 근심거리 듣기
        talkData.Add(110000 + 40, new string[] { "자네가 루나의 동전을 찾아주었다는 얘기를 들었네", "마을을 대표하는 촌장으로서 감사를 전하네", "그런데 우리 마을은 지금 큰 위험에 처했네", "얼마전부터 몬스터들이 나타나서 사람들을 위협하고 있네", "자네가 몬스터들을 무찔러 주면 정말 고맙겠네", "어려운 전투가 될테니 이 무기를 가져가게" });

        // 퀘스트 50: 몬스터 처치하기
        talkData.Add(110000 + 50, new string[] { "용감한 모험자여 자네만 믿고 있겠네", "부디 마을을 위험에서 건져주게" });
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