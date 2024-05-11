using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using BackEnd;

public class Nickname : LoginBase {

    [Serializable]
    public class NicknameEvent : UnityEvent { }

    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    private Image nicknameImage;

    [SerializeField]
    private InputField nicknameInputField;
    
    [SerializeField]
    private Button nicknameUpdateButton;

    private void OnEnable() { // 게임 오브젝트가 켜질때 호출되는 메소드
        // 닉네임 변경에 실패해 에러 메시지를 출력한 상태에서
        // 닉네임 변경 팝업을 닫았다가 열 수 있기 때문에 상태를 초기화
        ResetUI(nicknameImage);
        SetMessage("닉네임을 입력하세요");
    }

    public void OnClickUpdateNickname() {
        ResetUI(nicknameImage); // 파라미터로 입력한 InputField UI의 색상과 Message 내용 초기화

        // 필드 값이 비어있는지 체크
        if(IsFieldDataEmpty(nicknameImage, nicknameInputField.text, "Nickname")) return;

        nicknameUpdateButton.interactable = false;
        SetMessage("닉네임 변경중입니다...");
        UpdateNickname();
    }

    private void UpdateNickname() {
        Backend.BMember.UpdateNickname(nicknameInputField.text, callback => {

            nicknameUpdateButton.interactable = true; // 서버로부터 응답이 돌아오면 버튼을 다시 활성화 해주기
            if(callback.IsSuccess()) {
                SetMessage($"{nicknameInputField.text}(으)로 닉네임이 변경되었습니다.");
                onNicknameEvent?.Invoke();
            } else {
                string message = string.Empty;
                switch(int.Parse(callback.GetStatusCode())) {

                    case 400: // 빈 닉네임 혹은 string.Empty, 20자 이상의 닉네임, 닉네임 앞/뒤에 공백이 있는 경우
                        message = "닉네임이 비어있거나 | 20자 이상이거나 | 앞/뒤에 공백이 있습니다.";
                        break;

                    case 409: // 이미 중복된 닉네임이 있는 경우
                        message = "이미 존재하는 닉네임입니다.";
                        break;
                        
                    default:
                        message = callback.GetMessage();
                        break;
                }

                GuideForIncorrectlyEnteredData(nicknameImage, message);
            }
        });
    }

}