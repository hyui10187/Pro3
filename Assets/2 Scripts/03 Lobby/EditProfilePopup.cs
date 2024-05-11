using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditProfilePopup : MonoBehaviour {

    [SerializeField]
    private Text nicknameText;
    
    [SerializeField]
    private Text gamerIdText;

    public void UpdateNickname() {
        nicknameText.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
        gamerIdText.text = UserInfo.Data.gamerId;
    }

}