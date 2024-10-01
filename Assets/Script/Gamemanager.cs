using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance = null;
    public int life = 10;
    public int totalCoin = 0;
    public float Stage1totalTime = 200.0f;
    public float Stage2totalTime = 200.0f;
    public float Stage3totalTime = 200.0f;
    public float Stage4totalTime = 200.0f;


    public float clearTime;

    public float[] highestScore;

    public int playStage = 0;

    public AudioSource stageSource;

    public AudioClip menuMusic;
    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip stage3Music;
    public AudioClip stage4Music;

    public bool startBgm = false;

    public bool isHard = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < highestScore.Length; i++)
        {
            highestScore[i] = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        string sceneName = SceneManager.GetActiveScene().name;

        if (startBgm)
        {
            if(sceneName == "Menu")
            {
                stageSource.clip = menuMusic;
            }
            if (sceneName == "Stage1")
            {
                stageSource.clip = stage1Music;
            }
            if (sceneName == "Stage2")
            {
                stageSource.clip = stage2Music;
            }
            if (sceneName == "Stage3")
            {
                stageSource.clip = stage3Music;
            }
            if (sceneName == "BossStage")
            {
                stageSource.clip = stage4Music;
            }

            stageSource.Play();
            startBgm = false;
        }
    }
}
