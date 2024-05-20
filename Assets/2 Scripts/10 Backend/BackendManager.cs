using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.UI; // 뒤끝 SDK

public class BackendManager : MonoBehaviour {
    
    private void Awake() {
        DontDestroyOnLoad(gameObject); // Update 메소드의 Backend.AsyncPoll 호출을 위해 파괴하지 않는다
        BackendSetup(); // 뒤끝 서버 초기화
    }

    private void Update() {
        if(Backend.IsInitialized) {
            Backend.AsyncPoll();
            
        }
    }

    private void BackendSetup() {
        BackendReturnObject bro = Backend.Initialize(true); // 뒤끝 초기화

        if(bro.IsSuccess()) { // 뒤끝 초기화에 대한 응답값
            Debug.Log($"초기화 성공 : {bro}"); // 초기화 성공시 statusCode 204 Success
        } else {
            Debug.Log($"초기화 실패 : {bro}"); // 초기화 실패시 statusCode 400대 에러 발생
        }

    }
    
}