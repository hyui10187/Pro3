using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class Login : LoginBase { // MonoBehaviour 클래스가 아니라 LoginBase를 상속받기

    [SerializeField]
    private Image idImage;

    [SerializeField]
    private InputField idInputField;
    
    [SerializeField]
    private Image pwImage;

    [SerializeField]
    private InputField pwInputField;

    [SerializeField]
    private Button loginBtn;

    [SerializeField]
    private bool response;
    
    public void OnClickLogin() { // 로그인 버튼을 클릭했을때 호출되는 메소드
        ResetUI(idImage, pwImage); // ID와 PW InputField 색상과 Message를 초기화

        // InputField 값이 비어있는지 체크
        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.아이디.ToString())) return;
        if(IsFieldDataEmpty(pwImage, pwInputField.text, InputFieldType.비밀번호.ToString())) return;

        loginBtn.interactable = false; // 로그인 버튼을 연속적으로 클릭하지 못하도록 상호작용 비활성화
        StartCoroutine(LoginProcess()); // 서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        ResponseToLogin(idInputField.text, pwInputField.text); // 뒤끝 서버 로그인 시도
    }

    private void ResponseToLogin(string id, string pw) {
        
        // 서버에 로그인 요청(비동기)
        Backend.BMember.CustomLogin(id, pw, callback => {
            response = true; // 서버로부터 응답이 오면 로그인 로딩 애니메이션은 꺼주기
            
            if(callback.IsSuccess()) { // 로그인 성공시
                SetMessage($"{idInputField.text}님 환영합니다.");
                ChangeSceneManager.LoadScene(SceneNames.LobbyScene);

            } else {
                loginBtn.interactable = true; // 로그인에 실패했을 경우에는 다시 로그인을 해야하니 로그인 버튼을 다시 활성화 해주기
                string message = string.Empty;
                switch(int.Parse(callback.GetStatusCode())) {
                    case 401: // 존재하지 않는 아이디, 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;

                    case 403: // 유저 or 디바이스 차단
                        message = callback.GetMessage().Contains("user") ? "차단당한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    
                    case 410: // 탈퇴 진행중
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if(message.Contains("비밀번호")) { // StatusCode 401에서 "잘못된 비밀번호입니다." 일때
                    GuideForIncorrectlyEnteredData(pwImage, message);
                } else {
                    GuideForIncorrectlyEnteredData(idImage, message);
                }
            }
        });
    }

    private IEnumerator LoginProcess() {
        float time = 0;

        while(!response) {
            time += Time.deltaTime;
            SetMessage($"로그인 중입니다... {time:F1}");
            yield return null;
        }
    }
    
}