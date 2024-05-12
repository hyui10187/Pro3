using System;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.Events;

public class UserInfo : MonoBehaviour {

    [Serializable]
    public class UserInfoEvent : UnityEvent { }
    
    public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

    private static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data; // getter 메소드

    public void GetUserInfoFromBackend() { // 서버에서 유저의 정보를 불러오는 메소드
        Backend.BMember.GetUserInfo(callback => {
            if(callback.IsSuccess()) {
                try { // JSON 데이터 파싱 성공
                    JsonData json = callback.GetReturnValuetoJSON()["row"];
                    data.gamerId = json["gamerId"].ToString();
                    data.countryCode = json["countryCode"]?.ToString();
                    data.nickname = json["nickname"]?.ToString();
                    data.inDate = json["inDate"].ToString();
                    data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
                    data.subscriptionType = json["subscriptionType"].ToString();
                    data.federationId = json["federationId"]?.ToString();

                } catch(Exception e) { // JSON 데이터 파싱 실패
                    data.Reset(); // 유저 정보를 기본 상태로 설정
                    Debug.LogError(e); // try-catch 에러 출력
                }

            } else {
                data.Reset(); // 유저 정보를 기본 상태로 설정
                Debug.LogError(callback.GetMessage()); // Tip. 일반적으로 오프라인 상태를 대비해 기본적인 정보를 저장해두고 오프라인일때 불러와서 사용
            }
            onUserInfoEvent?.Invoke(); // 유저 정보 불러오기가 완료되었을때 onUserInfoEvent에 등록되어 있는 이벤트 메소드 호출
        });
    }
}

public class UserInfoData { // DB에서 가져온 유저 정보를 담아주기 위한 클래스
    
    public string gamerId; // 유저의 gamerID
    public string countryCode; // 국가코드. 설정 안했으면 null
    public string nickname; // 닉네임. 설정 안했으면 null
    public string inDate; // 유저의 inDate
    public string emailForFindPassword; // 이메일주소. 설정 안했으면 null
    public string subscriptionType; // 커스텀, 페더레이션 타입
    public string federationId; // 구글, 애플, 페이스북 페더레이션 ID. 커스텀 계정은 null

    public void Reset() {
        gamerId = "Offline";
        countryCode = "Unknown";
        nickname = "Noname";
        inDate = string.Empty;
        emailForFindPassword = string.Empty;
        subscriptionType = string.Empty;
        federationId = string.Empty;
    }
    
}