using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMove : MonoBehaviour
{
    public bool moveVertically = false;   // 縦方向の移動フラグ
    public bool moveHorizontally = false; // 横方向の移動フラグ
    public float moveSpeed = 2f;          // 移動速度
    public float rotationSpeed = 50f;     // 回転速度
    public float directionChangeInterval = 2f; // 移動方向を反転する間隔（秒）

    private Rigidbody2D rb;
    private float timer;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D コンポーネントを取得
        direction = Vector2.one;          // 初期移動方向を1（正の方向）に設定

        if (!Gamemanager.instance.isHard)
        {
            Destroy(this.gameObject);
        }

    }

    void Update()
    {
        // 時計回りの回転
        rb.angularVelocity = -rotationSpeed;  // 負の値で時計回り

        // 一定時間ごとに移動方向を反転
        timer += Time.deltaTime;
        if (timer > directionChangeInterval)
        {
            timer = 0f;
            direction *= -1;  // 移動方向を反転
        }

        // 移動処理
        Vector2 movement = Vector2.zero;

        if (moveVertically)
        {
            // 縦方向の速度を設定
            movement.y = direction.y * moveSpeed;
        }

        if (moveHorizontally)
        {
            // 横方向の速度を設定
            movement.x = direction.x * moveSpeed;
        }

        // Rigidbody2D の速度を設定
        rb.velocity = movement;
    }
}
