using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ClearScore : MonoBehaviour
{
    public TextMeshProUGUI[] resultText;   
    public float resultTime = 0.0f;
    private float timePoint;
    private float lifeBonus;
    private float coinBonus;
    private float totalScore;
    private float highScore;

    public AudioSource rollSource;
    public AudioSource resultSource;
    public AudioSource highscoreSource;

    public AudioClip rollSound;
    public AudioClip resultSound;
    public AudioClip highscoreSound;

    private bool resultFlag = false;
    private bool highscoreFlag = false;

    private bool skip = false;

    public GameObject pressSpace;
    public GameObject newRecord;

    public GameObject hardImage;

    public GameObject[] stageName;


    // Start is called before the first frame update
    void Start()
    {
        if (Gamemanager.instance.isHard)
        {
            hardImage.SetActive(true);
        }
        else
        {
            hardImage.SetActive(false);
        }

        pressSpace.SetActive(false);
        newRecord.SetActive(false);

        for(int i = 0; i < resultText.Length; i++)
        {
            resultText[i].text = null;
        }

        timePoint = Gamemanager.instance.clearTime;
        lifeBonus = Gamemanager.instance.life * 20;
        coinBonus = Gamemanager.instance.totalCoin * 10;
        totalScore = timePoint + lifeBonus + coinBonus;

        rollSource.Play();
        
        for(int i = 0; i < stageName.Length; i++)
        {
            stageName[i].SetActive(false);
        }

        stageName[Gamemanager.instance.playStage].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if(resultTime < 6.0f)
            {
                rollSource.Stop();
                resultTime = 6.0f;
            }
            else
            {
                Gamemanager.instance.startBgm = true;
                SceneManager.LoadScene("Menu");
                Gamemanager.instance.playStage = 0;
            }
        }

        if(Gamemanager.instance.playStage != 0)
        {
            resultText[4].text = Gamemanager.instance.highestScore[Gamemanager.instance.playStage].ToString("F2");

        }

        resultTime += Time.deltaTime;
        //それぞれにおいて音を鳴らす
        if (resultTime > 1.0f)
        {
            resultText[0].text = timePoint.ToString("F2");
        }
        if(resultTime > 2.0f)
        {
            resultText[1].text = lifeBonus.ToString("F2");
        }
        if(resultTime > 3.0f)
        {
            resultText[2].text = coinBonus.ToString("F2");
        }
        if(resultTime > 4.5f && !resultFlag)
        {
            resultSource.PlayOneShot(resultSound);
            resultFlag = true;
        }
        if(resultTime > 5.0f)
        {
            resultText[3].text = totalScore.ToString("F2");
        }
        if(resultTime > 6.0f && !highscoreFlag)
        {
            highscoreFlag = true;
            pressSpace.SetActive(true);
            if (totalScore > Gamemanager.instance.highestScore[Gamemanager.instance.playStage])
            {
                newRecord.SetActive(true);
                highscoreSource.PlayOneShot(highscoreSound);
                Gamemanager.instance.highestScore[Gamemanager.instance.playStage] = totalScore;
            }

        }

    }
}
