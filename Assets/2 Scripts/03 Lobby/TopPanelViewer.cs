using UnityEngine;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour {

    [SerializeField]
    private Text nickName;

    public void UpdateNickName() { // 설정된 닉네임이 있으면 그것을 가져오고 아니면 기본 gamerId를 넣어준다
        nickName.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

}