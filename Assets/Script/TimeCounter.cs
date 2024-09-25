using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    private float remainingTime;
    public PlayerMove playermove;
    // Start is called before the first frame update
    void Start()
    {
        Gamemanager.instance.clearTime = 0.0f;
        string sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "Stage1")
        {
            remainingTime = Gamemanager.instance.Stage1totalTime;
        }
        if(sceneName == "Stage2")
        {
            remainingTime = Gamemanager.instance.Stage1totalTime;
        }
        if (sceneName == "Stage3")
        {
            remainingTime = Gamemanager.instance.Stage1totalTime;
        }
        if (sceneName == "BossStage")
        {
            remainingTime = Gamemanager.instance.Stage1totalTime;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!playermove.timerStop)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            Gamemanager.instance.clearTime = remainingTime;
        }

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            Gamemanager.instance.life = 0;
        }
        else if (remainingTime <= 9.9f)
        {
            TimerText.text = remainingTime.ToString("F2");
        }
        else if (remainingTime <= 99.9f)
        {
            TimerText.text = remainingTime.ToString("F1");
        }
        else
        {
            TimerText.text = remainingTime.ToString("F0");
        }
    }
}
