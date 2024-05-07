using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class FindPW : LoginBase {

    [SerializeField]
    private Image idImage;

    [SerializeField]
    private InputField idInputField;

    [SerializeField]
    private Image emailImage;

    [SerializeField]
    private InputField emailInputField;
    
    [SerializeField]
    private Button findPwButton;

    public void OnClickFindPW() {
        ResetUI(idImage, emailImage);

        if(IsFieldDataEmpty(idImage, idInputField.text, InputFieldType.아이디.ToString())) return;
        if(IsFieldDataEmpty(emailImage, emailInputField.text, InputFieldType.이메일.ToString())) return;

        if(!emailInputField.text.Contains("@")) {
            GuideForIncorrectlyEnteredData(emailImage, "메일 형식이 잘못되었습니다.");
            return;
        }

        findPwButton.interactable = false;
        SetMessage("메일을 발송중입니다...");
        FindCustomPW();
    }

    private void FindCustomPW() {

        Backend.BMember.ResetPassword(idInputField.text, emailInputField.text, callback => {
            findPwButton.interactable = true; // 비밀번호 찾기 버튼을 다시 활성화 해주기
            
            if(callback.IsSuccess()) {
                SetMessage($"{emailInputField.text} 주소로 메일을 발송하였습니다.");
            } else {
                string message = string.Empty;
                
                switch(int.Parse(callback.GetStatusCode())) {
                    case 404: // 해당 이메일의 게이머가 없는 경우
                        message = "해당 이메일을 사용하는 사용자가 없습니다.";
                        break;
                    
                    case 429: // 24시간 이내에 5회 이상 같은 이메일 정보로 아이디/비밀번호 찾기를 시도한 경우
                        message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도했습니다.";
                        break;
                    
                    default: // statusCode : 400 => 프로젝트명에 특수문자가 추가된 경우 (안내 메일 미발송 및 에러 발생)
                        message = callback.GetMessage();
                        break;
                }

                if(message.Contains("이메일")) {
                    GuideForIncorrectlyEnteredData(emailImage, message);
                } else {
                    SetMessage(message);
                }
            }
        });

    }
    
    
}