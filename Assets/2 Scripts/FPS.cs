using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {
    
    public Text uiText; // 표시할 Text
    public float fps; // fps 프레임
    public float worstFps;

    private void Awake() {
        worstFps = 100;
        StartCoroutine("UpdateFPSPanel");
    }

    private void Update() {
        fps = 1 / Time.deltaTime;
        if(fps < worstFps) {
            worstFps = fps;
        }
    }

    private IEnumerator UpdateFPSPanel() {
        while(true) {
            yield return new WaitForSeconds(3f);
            uiText.text = "현재 프레임: " + fps.ToString("F0") + " / 최저 프레임: " + worstFps.ToString("F0");   
        }
    }

    // private void Awake() {
    //     StartCoroutine("WorstReset");
    // }
    
    // private void Update() {
    //     deltaTime += (Time.deltaTime - deltaTime) * 0.1f; // (현재 프레임 시간 - 이전 프레임 시간) X 0.1
    //     msec = deltaTime * 1000.0f;
    //     fps = 1.0f / deltaTime; // 초당 프레임 - 1초에
    //     if(fps < worstFps) { // 새로운 최저 fps가 나왔다면 기존의 worstFps를 업데이트 해주기
    //         worstFps = fps;
    //         uiText.text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") / worst : " + worstFps.ToString("F1");
    //     }
    // }
    
    // private IEnumerator WorstReset() { // 코루틴으로 15초 간격으로 최저 프레임을 100으로 리셋해주기
    //     while(true) {
    //         yield return new WaitForSeconds(15f);
    //         worstFps = 100f;
    //     }
    // }

}