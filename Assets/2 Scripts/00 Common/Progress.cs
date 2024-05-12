using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Progress : MonoBehaviour {

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private Text progressText;
    
    [SerializeField]
    private float progressTime; // 로딩바 재생시간

    public void Play(UnityAction action = null) { // 파라미터의 디폴트 값을 null로 설정하기
        StartCoroutine(Onprogress(action));
    }

    private IEnumerator Onprogress(UnityAction action) {

        float current = 0;
        float percent = 0;

        while(percent < 1) {
            current += Time.deltaTime;
            percent = current / progressTime;

            progressText.text = $"로딩중... {Mathf.RoundToInt(progressSlider.value * 100)}%"; // 로딩중 텍스트 뿌려주기
            progressSlider.value = Mathf.Lerp(0, 1, percent); // 로딩바의 Slider 갑 설정

            yield return null;
        }
        
        action?.Invoke(); // 파라미터로 받은 action이 null이 아니면 action 메소드 실행
    }

}