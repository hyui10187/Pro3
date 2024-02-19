using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour {

    public Camera minimapCamera;
    public float zoomMin;
    public float zoomMax;
    public float zoomOneStep;
    public Text mapNameText;

    private void Awake() {
        mapNameText.text = SceneManager.GetActiveScene().name; // 맵의 이름을 현재 씬의 이름으로 설정
    }

    public void ZoomIn() {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }

    public void ZoomOut() {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }
    
}