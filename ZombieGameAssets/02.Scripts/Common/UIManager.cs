using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //씬관련 기능을 쓰겠다라고 선언후 생략을 명시하다.
public class UIManager : MonoBehaviour
{
   
 
    public void PlayGame() //함수 :메서드 Method
    {
       SceneManager.LoadScene("MainScene");

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
 Application.Quit();
#endif

    }
}
