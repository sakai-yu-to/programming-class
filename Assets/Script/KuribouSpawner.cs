using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuribouSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject kuribouPrefab; // �N���{�[��Prefab
    public float spawnInterval = 2.0f; // �����Ԋu�i�b�j
    public Transform[] spawnPoints; // �N���{�[����������镡���̈ʒu�̔z��

    public float triggerDistance; // �v���C���[���߂Â�����
    private Vector3 originalPosition;
    private bool moveBigin = false;


    private float timer = 0.0f; // �o�ߎ��Ԃ�ǐՂ��邽�߂̃^�C�}�[

    private void Update()
    {
        float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer <= triggerDistance)
        {
            Debug.Log("fall goomba");
            moveBigin = true;
        }

        if (moveBigin)
        {
            // �o�ߎ��Ԃ𑝉�
            timer += Time.deltaTime;

            // �o�ߎ��Ԃ������Ԋu�𒴂����ꍇ�ɃN���{�[�𐶐�
            if (timer >= spawnInterval)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    Instantiate(kuribouPrefab, spawnPoints[i].position, Quaternion.identity);
                }

                timer = 0.0f; // �^�C�}�[�����Z�b�g
            }

        }

    }
}
