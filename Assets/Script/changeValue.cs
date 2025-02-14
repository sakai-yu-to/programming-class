using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeValue : MonoBehaviour
{
    [Header("プレイヤーの速度")]public float playerSpeed; //player
    [Header("プレイヤーのジャンプ速度")] public float playerJumpSpeed;
    [Header("プレイヤーのジャンプ力")] public float playerJumpHeight;
    [Header("プレイヤーにかかる重力")] public float playerGravity;

    [Header("クリボーの速度")] public float kuribouSpeed; //kuribou

    [Header("ドッスンの落下速度")] public float dossunFallSpeed; // dossun
    [Header("ドッスンの上昇速度")] public float dossunRiseSpeed;


    [Header("キラーの速度")] public float killerSpeed;  //killer
    [Header("金キラーの追尾間隔")] public float goldkillerCheckCycle;  // 追尾チェック周期
        
    [Header("歯車の速度")] public float gearmoveSpeed; //gear


    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 10;
        playerJumpSpeed = 15;
        playerJumpHeight = 4;
        playerGravity = 7;
        kuribouSpeed = 1.5f;
        dossunFallSpeed = 30;
        dossunRiseSpeed = 4;
        killerSpeed = 5;
        goldkillerCheckCycle = 2;
        gearmoveSpeed = 5;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerSpeed = 10;
            playerJumpSpeed = 15;
            playerJumpHeight = 4;
            playerGravity = 7;
            kuribouSpeed = 1.5f;
            dossunFallSpeed = 30;
            dossunRiseSpeed = 4;
            killerSpeed = 5;
            goldkillerCheckCycle = 2;
            gearmoveSpeed = 5;
        }

    }
}
