using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneNames { LoadingScene, MainScene, LoginScene, LobbyScene }

public enum InputFieldType { 아이디, 비밀번호, 비밀번호확인, 이메일 }

public static class ChangeSceneManager {

    public static string GetActiveScene() {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadScene(string sceneName = "") {

        if(sceneName == "") {
            SceneManager.LoadScene(GetActiveScene());
        } else {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void LoadScene(SceneNames sceneNames) {
        SceneManager.LoadScene(sceneNames.ToString()); // SceneNames 열거형으로 매개변수를 받아온 경우 ToSting 메소드를 이용해서 문자열로 변환해주기
    }
    
}