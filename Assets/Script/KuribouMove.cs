using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuribouMove : MonoBehaviour
{
    public GameObject player;
    public float kuribouSpeed; // 左右の移動速度
    private float kuribouxSpeed;
    private Animator kuribouAnim = null;
    private Rigidbody2D kuribourb = null;
    private bool hasTouchedGround = false; // 地面に接触したかどうかを記録するフラグ
    public int turn = 0;
    public float triggerDistance; // プレイヤーが近づく距離
    private Vector3 originalPosition; 
    private bool moveBigin = false;


    void Start()
    {
        originalPosition = transform.position; // 元の位置を保存
        kuribourb = GetComponent<Rigidbody2D>();
        kuribouAnim = GetComponent<Animator>();
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned in the inspector.");
        }
    }

    void Update()
    {
        float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer <= triggerDistance)
        {
            moveBigin = true;
        }


        if (moveBigin)
        {
            if (hasTouchedGround)
            {
                // 地面に接触した後の左右の移動処理
                if (turn % 2 == 0)
                {
                    transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    kuribouxSpeed = -kuribouSpeed;
                }
                else
                {
                    transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                    kuribouxSpeed = kuribouSpeed;
                }

                kuribourb.velocity = new Vector2(kuribouxSpeed, kuribourb.velocity.y);
            }
            else
            {
                // 地面に接触するまでは真下に落下する
                kuribourb.velocity = new Vector2(0, kuribourb.velocity.y);
            }

        }
        else
        {
            kuribourb.velocity = new Vector2(0, 0);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            hasTouchedGround = true; // "Ground"に接触したら、左右の移動を開始する
        }

        if (collision.collider.CompareTag("Turn") || collision.collider.CompareTag("Kuribou"))
        {
            turn++;
            Debug.Log("Kuribou turned");
        }

        if (collision.collider.CompareTag("Dead"))
        {
            Destroy(this.gameObject);
        }
    }
}
