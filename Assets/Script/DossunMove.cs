using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossunMove : MonoBehaviour
{
    public GameObject player; // プレイヤーの参照
    public float fallSpeed = 5f; // 落下速度
    public float riseSpeed = 2f; // 上昇速度
    public float triggerDistance; // プレイヤーが近づく距離
    private Vector3 originalPosition; // ドッスンの元の位置
    private bool isFalling = false; // ドッスンが落下中かどうか
    private bool isRising = false; // ドッスンが上昇中かどうか
    private bool isWaiting = false; // 待機中かどうか



    private Rigidbody2D rb;

    void Start()
    {
        originalPosition = transform.position; // 元の位置を保存
        rb = GetComponent<Rigidbody2D>();

        // Z軸の回転をロックしてドッスンが傾かないようにする
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!isFalling && !isRising && !isWaiting)
        {
            float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);

            // プレイヤーが近づいたら落下を開始
            if (distanceToPlayer <= triggerDistance)
            {
                isFalling = true;
            }
        }

        if (isFalling)
        {
            Fall();
        }
        else if (isRising)
        {
            Rise();
        }
    }

    void Fall()
    {
        // ドッスンを落下させる（Rigidbody2Dを使うことで物理的な落下を処理）
        rb.velocity = Vector2.down * fallSpeed;
    }

    void Rise()
    {
        // ドッスンを上限の高さまで上昇させる
        rb.velocity = Vector2.up * riseSpeed;

        // Y座標が元の位置に近づいたら、X座標を調整しながら停止する
        if (transform.position.y >= originalPosition.y)
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
            isRising = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に触れたら落下を停止し、2秒待機後に上昇を開始
        if (collision.collider.CompareTag("Ground"))
        {
            if (isFalling)
            {
                isFalling = false;
                rb.velocity = Vector2.zero;
                StartCoroutine(WaitBeforeRise());
            }
        }

        if (collision.collider.CompareTag("Kuribou"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.collider.CompareTag("Dead"))
        {
            Destroy(this.gameObject);
        }

    }

    IEnumerator WaitBeforeRise()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        isWaiting = false;
        isRising = true;
    }
}
