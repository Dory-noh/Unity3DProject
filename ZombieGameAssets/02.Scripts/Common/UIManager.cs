using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //������ ����� ���ڴٶ�� ������ ������ ����ϴ�.
public class UIManager : MonoBehaviour
{
   
 
    public void PlayGame() //�Լ� :�޼��� Method
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
