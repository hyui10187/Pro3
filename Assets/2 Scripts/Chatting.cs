using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chatting : MonoBehaviour
{
    [SerializeField] private Transform chatParent; // 채팅 객체를 가지고 있을 부모
    [SerializeField] private GameObject chatPrefab; // 입력한 채팅을 표시해줄 채팅 프리팹
    [SerializeField] private TMP_InputField inputField; // 채팅 입력창

    public void OnEnter()
    {
        GameObject chatObj = Instantiate(chatPrefab, chatParent);
        chatObj.GetComponent<TextMeshProUGUI>().text = $"{DateTime.Now.ToString("hh:mm:ss")}";
        chatObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = inputField.text;
    }

}