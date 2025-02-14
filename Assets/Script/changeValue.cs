using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeValue : MonoBehaviour
{
    [Header("�v���C���[�̑��x")]public float playerSpeed; //player
    [Header("�v���C���[�̃W�����v���x")] public float playerJumpSpeed;
    [Header("�v���C���[�̃W�����v��")] public float playerJumpHeight;
    [Header("�v���C���[�ɂ�����d��")] public float playerGravity;

    [Header("�N���{�[�̑��x")] public float kuribouSpeed; //kuribou

    [Header("�h�b�X���̗������x")] public float dossunFallSpeed; // dossun
    [Header("�h�b�X���̏㏸���x")] public float dossunRiseSpeed;


    [Header("�L���[�̑��x")] public float killerSpeed;  //killer
    [Header("���L���[�̒ǔ��Ԋu")] public float goldkillerCheckCycle;  // �ǔ��`�F�b�N����
        
    [Header("���Ԃ̑��x")] public float gearmoveSpeed; //gear


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
