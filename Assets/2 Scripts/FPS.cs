using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

    public static FPS instance;
    
    public Text uiText; // 표시할 Text
    public float fps; // fps 프레임
    public float worstFps;

    private void Awake() {
        worstFps = 100;
        instance = this;
    }

    private void Update() {
        fps = 1 / Time.deltaTime;
        if(fps < worstFps) {
            worstFps = fps;
        }
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