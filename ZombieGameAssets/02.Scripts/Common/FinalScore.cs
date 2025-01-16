using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    public Text KillScore;
    // Start is called before the first frame update
    void Start()
    {
        KillScore.text = $"KillScore: <color=#ff0000>{GameManager.Instance.gameData.killCount.ToString()}</color>";
    }


}
