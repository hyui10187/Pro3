using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginBase : MonoBehaviour {

    [SerializeField]
    private Text message;

    protected void ResetUI(params Image[] images) { // 입력을 안한 InputField가 여러개일 수 있으니 params 키워드를 통해서 파라미터를 가변적으로 받아주기
        message.text = string.Empty; // 메시지 내용 비워주기

        for(int i = 0; i < images.Length; i++) {
            images[i].color = Color.white; // InputField 색상 초기화
        }
    }

    protected void SetMessage(string msg) { // 접근제한자를 protected로 선언해서 자식 클래스에서 호출이 가능하도록 해주기
        message.text = msg; // 파라미터로 받아온 내용을 출력해주기
    }

    protected void GuideForIncorrectlyEnteredData(Image image, string msg) {
        message.text = msg;
        image.color = Color.red; // InputField 색상을 빨간색으로 표시해주기
    }

    protected bool IsFieldDataEmpty(Image image, string field, string result) {

        if(field.Trim().Equals("")) { // InputField에 아무것도 입력하지 않았을 경우에는
            GuideForIncorrectlyEnteredData(image, $"{result}를 입력하세요.");
            return true;
        }
        
        return false;
    }
    
}