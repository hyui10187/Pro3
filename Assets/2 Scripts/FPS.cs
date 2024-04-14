using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

    public static FPS instance;
    
    public Text uiText; // FPS를 표시할 Text 부분
    public float fps; // 화면의 초당 프레임 수

    private void Awake() {
        instance = this;
    }

    private void Update() {
        fps = 1 / Time.smoothDeltaTime;
    }

    public void ControlFPSPanel() {
        StopCoroutine("UpdateFPSPanel");
        StartCoroutine("UpdateFPSPanel");
    }

    public void StopControlFPSPanel() {
        StopCoroutine("UpdateFPSPanel");
    }

    private IEnumerator UpdateFPSPanel() { // 3초 간격으로 FPS를 갱신해주는 코루틴
        while(true) {
            yield return new WaitForSeconds(3f);
            uiText.text = fps.ToString("F0") + " FPS";
        }
    }

}