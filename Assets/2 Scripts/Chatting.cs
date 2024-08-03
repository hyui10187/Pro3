using System;
using TMPro;
using UnityEngine;

public class Chatting : MonoBehaviour
{
    [SerializeField] private Transform chatParent; // 채팅 객체를 가지고 있을 부모
    [SerializeField] private GameObject chatPrefab; // 입력한 채팅을 표시해줄 채팅 프리팹
    [SerializeField] private TMP_InputField chatInputField; // 채팅 입력창

    public void OnEnter() // 채팅창 Input Field에서 Enter 키를 입력했을 경우 호출할 메소드
    {
        if(!Input.GetKeyDown(KeyCode.Return)) // Enter 키를 입력한게 아니라면 돌려보내기
            return;

        GameObject chatObj = Instantiate(chatPrefab, chatParent);
        chatObj.GetComponent<TextMeshProUGUI>().text = $"{DateTime.Now:HH:mm:ss}";
        chatObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = chatInputField.text;
        chatInputField.text = ""; // 채팅을 전송하고 나면 Input Field에 입력된 내용은 지워주기
        chatInputField.ActivateInputField(); // 채팅을 전송하고 나서도 Input Field에 포커스 유지하기
    }

    public void OnDeselect() // 채팅창 Input Field에서 포커스가 벗어났을 경우 호출할 메소드
    {
        chatInputField.text = ""; // Input Field에 입력된 내용을 지워주기
    }
    
}