using System;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.Events;

public class BackendGameData {

    private static BackendGameData instance;

    public static BackendGameData Instance {
        get {
            if(instance == null) 
                instance = new BackendGameData();
            
            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;
    
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private string gameDataRowInDate = string.Empty;

    public void GameDataLoad() { // 서버의 DB에서 유저의 정보를 가져오는 메소드
        Backend.GameData.GetMyData("TB_USER", new Where(), callback => {
            if(callback.IsSuccess()) {
                Debug.Log($"계정 정보를 불러오는데 성공하였습니다. : {callback}");
                try {
                    JsonData gameDataJson = callback.FlattenRows();
                    
                    if(gameDataJson.Count == 0) { // 받아온 데이터의 갯수가 0이면 데이터가 없는것
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    } else {
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.exp = float.Parse(gameDataJson[0]["exp"].ToString());
                        userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        
                        onGameDataLoadEvent?.Invoke();
                    }
                } catch(Exception e) {
                    userGameData.Reset();
                    Debug.LogError(e);
                }

            } else {
                Debug.LogError($"계정 정보를 불러오는데 실패하였습니다. : {callback}");
            }
        });
    }

    public void GameDataInsert() { // 서버의 DB에 유저의 정보를 저장해주는 메소드
        userGameData.Reset(); // 유저 정보를 초기값으로 설정

        // 컬럼명과 컬럼값을 셋팅해주기
        Param param = new Param() {
            { "level", userGameData.level },
            { "exp", userGameData.exp },
            { "gold", userGameData.gold },
        };

        // 첫번째 파라미터: 테이블 이름, 두번째 파라미터: 저장할 데이터, 세번째 파라미터: 콜백 함수
        Backend.GameData.Insert("TB_USER", param, callback => {
            if(callback.IsSuccess()) {
                gameDataRowInDate = callback.GetInDate();
                Debug.Log($"유저 정보를 저장하는데 성공하였습니다. : {callback}");
            } else {
                Debug.LogError($"유저 정보를 저장하는데 실패하였습니다.");
            }
        });
    }

}