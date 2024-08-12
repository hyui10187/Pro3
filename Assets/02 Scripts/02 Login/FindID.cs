using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class FindID : LoginBase {

    [SerializeField]
    private Image emailImage;

    [SerializeField]
    private InputField emailInputField;
    
    [SerializeField]
    private Button findIdButton; // 아이디 찾기 버튼

    public void OnClickFindID() {
        ResetUI(emailImage);

        // 이메일 InputField를 입력했는지 확인해주기
        if(IsFieldDataEmpty(emailImage, emailInputField.text, InputFieldType.이메일.ToString())) return;

        if(!emailInputField.text.Contains("@")) { // 골뱅이를 포함하고 있지 않으면
            GuideForIncorrectlyEnteredData(emailImage, "이메일 형식이 잘못되었습니다.");
            return;
        }

        findIdButton.interactable = false;
        SetMessage("메일 발송중입니다...");
        FindCustomID();
    }

    private void FindCustomID() {
        Backend.BMember.FindCustomID(emailInputField.text, callback => {
            findIdButton.interactable = true;
            if(callback.IsSuccess()) {
                SetMessage($"{emailInputField.text} 주소로 메일을 발송하였습니다.");
            } else {
                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode())) {
                    case 404: // 해당 이메일의 게이머가 없는 경우
                        message = "해당 이메일을 사용하는 사용자가 없습니다.";
                        break;

                    case 429: // 24시간 이내에 5회 이상 이메일 정보로 아이디/비밀번호 찾기를 시도한 경우
                        message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도했습니다.";
                        break;

                    default: // status : 400 => 프로젝트명에 특수문자가 추가된 경우 (안내 메일 미발송 및 에러 발생)
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