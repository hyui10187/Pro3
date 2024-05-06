using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class Register : LoginBase {

    [SerializeField]
    private Image idImage; // ID InputField 이미지

    [SerializeField]
    private InputField idInputField; // ID InputField
    
    [SerializeField]
    private Image pwImage;

    [SerializeField]
    private InputField pwInputField;
    
    [SerializeField]
    private Image pwConfirmImage;

    [SerializeField]
    private InputField pwConfirmInputField;

    [SerializeField]
    private Image emailImage;

    [SerializeField]
    private InputField emailInputField;
    
    [SerializeField]
    private Button registerButton; // 회원가입 버튼

    public void OnClickRegister() { // 회원가입 버튼을 클릭했을때 호출하는 메소드
        ResetUI(idImage, pwImage, pwConfirmImage, emailImage); // ResetUI 메소드는 params 키워드로 Image의 배열을 파라미터로 받고 있어서 이렇게 동적으로 파라미터 갯수를 설정가능

        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.아이디.ToString())) return;
        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.비밀번호.ToString())) return;
        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.비밀번호확인.ToString())) return;
        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.이메일.ToString())) return;

        if(!pwInputField.text.Equals(pwConfirmInputField.text)) { // 비밀번호와 비밀번호 확인 창이 다를 경우
            GuideForIncorrectlyEnteredData(pwConfirmImage, "비밀번호가 일치하지 않습니다.");
            return;
        }

        if(!emailInputField.text.Contains("@")) {
            GuideForIncorrectlyEnteredData(emailImage, "메일 형식이 잘못되었습니다.");
            return;
        }

        registerButton.interactable = false; // 회원가입 버튼을 여러번 클릭하는 것을 막기 위해 비활성화
        SetMessage("계정을 생성중입니다...");
        
        // 뒤끝 서버 계정생성 시도
        CustomSignUp();
    }

    private void CustomSignUp() { // 회원가입을 해주는 메소드
        
        Backend.BMember.CustomSignUp(idInputField.text, pwInputField.text, callback => {
            registerButton.interactable = true; // 회원가입 버튼을 다시 활성화 시켜주기
            
            if(callback.IsSuccess()) { // 계정생성 성공
                Backend.BMember.UpdateCustomEmail(emailInputField.text, callbackParam => {
                    if(callbackParam.IsSuccess()) { // 이메일을 업데이틑 하는 것에 성공했으면
                        SetMessage($"회원가입이 완료되었습니다. {idInputField.text}님 환영합니다.");
                    }
                });
            } else { // 계정생성 실패
                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode())) {
                    case 409: // 중복된 아이디가 있는 경우
                        message = "이미 존재하는 아이디입니다.";
                        break;

                    case 400: // 디바이스 정보가 null일 경우
                    case 401: // 프로젝트 상태가 '점검'일 경우
                    case 403: // 차단당한 디바이스일 경우
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if(message.Contains("아이디")) { // 아이디 중복에 대한 문제인 경우 아이디 InputField 색상을 빨갛게 하고 메세지를 넣어주기
                    GuideForIncorrectlyEnteredData(idImage, message);
                } else {
                    SetMessage(message);
                }
            }
        });
    }
    
}