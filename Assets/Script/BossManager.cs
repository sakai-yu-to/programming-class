using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossManager : MonoBehaviour
{
    public EnemyKill enemyKill;
    public GameObject[] hardFire;
    public GameObject[] easyFire;
    public Transform[] warpPointsBefore;
    public Transform[] warpPointsAfter;
    public GameObject[] hardHammer;
    public GameObject[] easyHammer;
    public Tilemap hammerLock;
    public Tilemap bossLock;
    public GameObject[] bossSituation;
    public int bossStatus;
    public GameObject[] easybosshammer;
    public GameObject[] hardbosshammer;

    private bool hammerdeleted = false;
    private bool changeStatus = false;

    public float bossDownLimitedTime = 5.0f;
    private float bossDownTime = 0;

    private bool bosskilled = false;

    // Start is called before the first frame update
    void Start()
    {
        bossSituation[1].SetActive(false);

        if (!Gamemanager.instance.isHard)
        {
            for (int i = 0; i < hardFire.Length; i++)
            {
                Destroy(hardFire[i]);
            }
            for(int i = 0; i< hardHammer.Length; i++)
            {
                Destroy(hardHammer[i]);
            }
            for(int i = 0; i< hardbosshammer.Length; i++)
            {
                Destroy(hardbosshammer[i]);
            }
            Destroy(hammerLock.gameObject);
        }
        else
        {
            for (int i = 0; i < hardFire.Length; i++)
            {
                FireBarMove fireBarMove = hardFire[i].GetComponent<FireBarMove>();
                if (fireBarMove != null)
                {
                    fireBarMove.isClockwise = Random.value > 0.5f; // ランダムにtrue/falseを設定
                    fireBarMove.rotationSpeed = Random.Range(100, 401); // 100〜400の間のランダムな値を設定
                }
            }
            for (int i = 0; i < easyFire.Length; i++)
            {
                FireBarMove fireBarMove = easyFire[i].GetComponent<FireBarMove>();
                if (fireBarMove != null)
                {
                    fireBarMove.isClockwise = Random.value > 0.5f; // ランダムにtrue/falseを設定
                    fireBarMove.rotationSpeed = Random.Range(100, 301); // 100〜300の間のランダムな値を設定
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyKill.killHammer == hardHammer.Length + easyHammer.Length && !hammerdeleted)
        {
            Destroy(hammerLock.gameObject);
            hammerdeleted = true;
        }

        if (enemyKill.touchLamp && !changeStatus && bossStatus < 5)
        {
            bossStatus++;
            changeStatus = true;
        }

        // 現在のオブジェクトの位置を取得
        Vector3 currentPosition = bossSituation[bossStatus % 2 == 0 ? 0 : 1].transform.position;

        if (bossStatus % 2 == 0)
        {
            bossSituation[1].SetActive(false);
            bossSituation[0].SetActive(true);
            bossSituation[0].transform.position = currentPosition;  // 位置を引き継ぐ

        }
        else if(bossStatus < 5)
        {
            bossSituation[0].SetActive(false);
            bossSituation[1].SetActive(true);
            bossSituation[1].transform.position = currentPosition;  // 位置を引き継ぐ
            bossDownTime += Time.deltaTime;
            if (bossDownTime > bossDownLimitedTime)
            {
                enemyKill.touchLamp = false;
                changeStatus = false;
                bossDownTime = 0;
                bossStatus++;

            }

        }


        if(bossStatus == 5 && !bosskilled)
        {
            Destroy(bossLock.gameObject);
            bosskilled = true;
        }
    }

    public void WarpPlayer(Transform player, int doorIndex)
    {
       player.position = warpPointsAfter[doorIndex].position;
    }
}
