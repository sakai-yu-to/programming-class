using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillerMove : MonoBehaviour
{
    public GameObject playermove;  // プレイヤーを指定
    public float killerSpeed;  // 敵の移動スピード
    private Rigidbody2D killerrb;
    private Animator killerAnim;

    public bool isGold = false;  // プレイヤーを追尾するか
    public bool goRight = false;  // 右に移動するか

    public float checkCycle;  // 追尾チェック周期
    private float trackingTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        killerrb = GetComponent<Rigidbody2D>();
        killerAnim = GetComponent<Animator>();
        playermove = GameObject.Find("Player");  // プレイヤーを探す

        // 初期向きの設定
        UpdateScaleAndVelocity();  // 初期のスケールと速度を更新
    }

    // Update is called once per frame
    void Update()
    {
        if (isGold)
        {
            // プレイヤーを追尾するロジック
            trackingTime += Time.deltaTime;
            if (trackingTime >= checkCycle)
            {
                trackingTime = 0.0f;

                // プレイヤーの方向を計算
                Vector2 direction = (playermove.transform.position - transform.position).normalized;
                killerrb.velocity = direction * killerSpeed;

                // スプライトの向きを設定
                UpdateScaleAndRotation(direction);
            }
        }
        else
        {
            // goRightがtrueなら右、falseなら左に移動
            UpdateScaleAndVelocity();  // スケールと速度を更新
        }
    }

    private void UpdateScaleAndVelocity()
    {
        Vector3 localScale = transform.localScale;
        if (goRight)
        {
            localScale.x = -0.15f;  // 右向きにスケール
            killerrb.velocity = new Vector2(killerSpeed, 0);  // 右に移動
        }
        else
        {
            localScale.x = 0.15f;  // 左向きにスケール
            killerrb.velocity = new Vector2(-killerSpeed, 0);  // 左に移動
        }
        transform.localScale = localScale;
    }

    private void UpdateScaleAndRotation(Vector2 direction)
    {
        // スプライトの向きを設定
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-0.15f, 0.15f, 1);  // 右向きにスケール
        }
        else
        {
            transform.localScale = new Vector3(0.15f, 0.15f, 1);  // 左向きにスケール]
            direction.y *= -1;
            direction.x *= -1;
        }

        // プレイヤーに向かって回転
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // ラジアンを度に変換
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  // 回転を設定

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Dead"))
        {
            Destroy(this.gameObject);  // "Dead"タグのオブジェクトに衝突したら自身を破壊
        }
    }
}
