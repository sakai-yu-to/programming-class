using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class KuribouSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject kuribouPrefab; // �N���{�[��Prefab
    public float spawnInterval = 2.0f; // �����Ԋu�i�b�j
    public Transform[] spawnPoints; // �N���{�[����������镡���̈ʒu�̔z��

    public float triggerDistance = 20.0f; // �v���C���[���߂Â�����
    private Vector3 originalPosition;
    private bool[] moveBigin;


    private float[] timer; // �o�ߎ��Ԃ�ǐՂ��邽�߂̃^�C�}�[

    private void Start()
    {
        moveBigin = new bool[spawnPoints.Length];
        timer = new float[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            moveBigin[i] = false;
            timer[i] = 0;
        }
    }

    private void Update()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            float distanceToPlayer = Mathf.Abs(player.transform.position.x - spawnPoints[i].transform.position.x);
            if (distanceToPlayer <= triggerDistance)
            {
                Debug.Log("fall goomba");
                moveBigin[i] = true;
            }

            if (moveBigin[i])
            {
                // �o�ߎ��Ԃ𑝉�
                timer[i] += Time.deltaTime;

                // �o�ߎ��Ԃ������Ԋu�𒴂����ꍇ�ɃN���{�[�𐶐�
                if (timer[i] >= spawnInterval)
                {
                    Instantiate(kuribouPrefab, spawnPoints[i].position, Quaternion.identity);
                    timer[i] = 0.0f; // �^�C�}�[�����Z�b�g
                }

            }

        }


    }
}
