using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class KuribouSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject kuribouPrefab; // クリボーのPrefab
    public float spawnInterval = 2.0f; // 生成間隔（秒）
    public Transform[] spawnPoints; // クリボーが生成される複数の位置の配列

    public float triggerDistance = 20.0f; // プレイヤーが近づく距離
    private Vector3 originalPosition;
    private bool[] moveBigin;


    private float[] timer; // 経過時間を追跡するためのタイマー

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
                // 経過時間を増加
                timer[i] += Time.deltaTime;

                // 経過時間が生成間隔を超えた場合にクリボーを生成
                if (timer[i] >= spawnInterval)
                {
                    Instantiate(kuribouPrefab, spawnPoints[i].position, Quaternion.identity);
                    timer[i] = 0.0f; // タイマーをリセット
                }

            }

        }


    }
}
