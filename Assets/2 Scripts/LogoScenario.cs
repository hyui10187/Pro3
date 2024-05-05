using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScenario : MonoBehaviour {

    [SerializeField]
    private Progress progress;

    [SerializeField]
    private SceneNames nextScene; // enum 타입의 변수
    
    private void Awake() {
        SystemSetup();
    }

    private void SystemSetup() {
        Application.runInBackground = true; // 활성화되지 않은 상태에서도 게임이 계속 진행
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 거지지 않도록 설정
        progress.Play(OnAfterProgress); // 로딩 애니메이션 시작, 재생 완료시 OnAfterProgress 메소드 실행
    }

    private void OnAfterProgress() {
        ChangeSceneManager.LoadScene(nextScene); // 파라미터로 enum 타입을 던져주기
    }
    
}