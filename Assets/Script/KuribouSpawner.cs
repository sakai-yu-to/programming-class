using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuribouSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject kuribouPrefab; // クリボーのPrefab
    public float spawnInterval = 2.0f; // 生成間隔（秒）
    public Transform[] spawnPoints; // クリボーが生成される複数の位置の配列

    public float triggerDistance; // プレイヤーが近づく距離
    private Vector3 originalPosition;
    private bool moveBigin = false;


    private float timer = 0.0f; // 経過時間を追跡するためのタイマー

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
            // 経過時間を増加
            timer += Time.deltaTime;

            // 経過時間が生成間隔を超えた場合にクリボーを生成
            if (timer >= spawnInterval)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    Instantiate(kuribouPrefab, spawnPoints[i].position, Quaternion.identity);
                }

                timer = 0.0f; // タイマーをリセット
            }

        }

    }
}
