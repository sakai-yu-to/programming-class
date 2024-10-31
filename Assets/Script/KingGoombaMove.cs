using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGoombaMove : MonoBehaviour
{
    public GameObject player;
    private float kuribouxSpeed;
    private Animator kuribouAnim = null;
    private Rigidbody2D kuribourb = null;
    private bool hasTouchedGround = false; // 地面に接触したかどうかを記録するフラグ
    public int turn = 0;

    public changeValue changevalue;

    void Start()
    {
        GameObject gamemanager = GameObject.Find("Gamemanager");
        if (gamemanager != null)
        {
            changevalue = gamemanager.GetComponent<changeValue>();
        }

        kuribourb = GetComponent<Rigidbody2D>();
        kuribouAnim = GetComponent<Animator>();
    }

    void Update()
    {
      
        if (hasTouchedGround)
        {
            // 地面に接触した後の左右の移動処理
            if (turn % 2 == 0)
            {
                transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                kuribouxSpeed = -changevalue.kuribouSpeed;
            }
            else
            {
                transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                kuribouxSpeed = changevalue.kuribouSpeed;
            }

            kuribourb.velocity = new Vector2(kuribouxSpeed, kuribourb.velocity.y);
        }
        else
        {
            // 地面に接触するまでは真下に落下する
            kuribourb.velocity = new Vector2(0, kuribourb.velocity.y);
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
