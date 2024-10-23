using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMove : MonoBehaviour
{
    public GameObject player;
    public float fallSpeed = 5f; // 落下速度
    public float riseSpeed = 2f; // 上昇速度
    public float triggerDistance; // プレイヤーが近づく距離
    public float waitTime = 2f; // 待機時間
    public float riseHeight = 5f; // 上昇するY座標の差

    private bool isFalling = false; // 落下中かどうか
    private bool isRising = false; // 上昇中かどうか
    private bool isWaiting = false; // 待機中かどうか

    private Rigidbody2D rb;
    private float originalY; // 元のY座標
    private float waitTimer = 0f; // 待機タイマー

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        originalY = transform.position.y; // 元のY座標を保持
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFalling && !isRising && !isWaiting)
        {
            float distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayerX <= triggerDistance)
            {
                isRising = true; // プレイヤーが近づくとまず上昇
            }
        }

        if (isRising)
        {
            rb.velocity = Vector2.up * riseSpeed;
            transform.localScale = new Vector3(0.25f, -0.25f, 0.25f);
            // 指定の高さに達したら落下開始
            if (transform.position.y >= originalY + riseHeight)
            {
                isRising = false;
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            rb.velocity = Vector2.down * fallSpeed;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            // 元のY座標まで戻ったら停止して待機状態へ移行
            if (transform.position.y <= originalY)
            {
                transform.position = new Vector2(transform.position.x, originalY); // 座標を正確に元に戻す
                rb.velocity = Vector2.zero; // 速度をゼロにして停止
                isFalling = false;
                isWaiting = true;
                waitTimer = waitTime; // 待機時間をセット
            }
        }
        else if (isWaiting)
        {
            waitTimer -= Time.deltaTime; // タイマーを減らす

            // 一定時間待機したら再びプレイヤーに反応する
            if (waitTimer <= 0f)
            {
                isWaiting = false; // 待機終了後、再びプレイヤーに反応する状態に戻る
            }
        }
    }
}
