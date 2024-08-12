using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneManager : MonoBehaviour
{

    public void OnLoginButtonClick() // 로그인하기 버튼을 클릭했을 경우 호출되는 메소드
    {
        SceneManager.LoadScene("LoginScene"); // 로그인 씬으로 이동하기
    }
    
    public void OnImmediateButtonClick() // 바로하기 버튼을 클릭했을 경우 호출되는 메소드
    {
        SceneManager.LoadScene("MainScene"); // 메인 씬으로 이동하기
    }
    
}