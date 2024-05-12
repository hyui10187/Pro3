using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI nickName;
    
    [SerializeField]
    private TextMeshProUGUI levelText;
    
    [SerializeField]
    private Slider expSlider;

    [SerializeField]
    private TextMeshProUGUI goldText;

    private void Awake() {
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    public void UpdateNickName() { // 설정된 닉네임이 있으면 그것을 가져오고 아니면 기본 gamerId를 넣어준다
        nickName.text = UserInfo.Data.nickname == null ? $"닉네임 : {UserInfo.Data.gamerId}" : $"닉네임 : {UserInfo.Data.nickname}";
    }

    public void UpdateGameData() {
        levelText.text = $"LV. {BackendGameData.Instance.UserGameData.level}";
        expSlider.value = BackendGameData.Instance.UserGameData.exp / 100;
        goldText.text = BackendGameData.Instance.UserGameData.gold.ToString();
    }

    public void EnterButtonClick() { // 입장하기 버튼을 클릭했을때 호출되는 메소드
        ChangeSceneManager.LoadScene(SceneNames.MainScene); // 메인씬으로 이동시켜 주기
    }

}