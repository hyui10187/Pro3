using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditProfilePopup : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI nicknameText;
    
    [SerializeField]
    private TextMeshProUGUI gamerIdText;

    public void UpdateNickname() {
        nicknameText.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
        gamerIdText.text = UserInfo.Data.gamerId;
    }

}