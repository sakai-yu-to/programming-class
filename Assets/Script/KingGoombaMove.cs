using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGoombaMove : MonoBehaviour
{
    public GameObject player;
    public float kuribouSpeed; // 左右の移動速度
    private float kuribouxSpeed;
    private Animator kuribouAnim = null;
    private Rigidbody2D kuribourb = null;
    private bool hasTouchedGround = false; // 地面に接触したかどうかを記録するフラグ
    private string turnTag = "Turn";
    private string groundTag = "Ground";
    private string kuribouTag = "Kuribou";
    private string bodyTag = "Body";
    private string headTag = "Head";
    private string footTag = "Foot";
    private string death = "Dead";
    public int turn = 0;


    void Start()
    {
        kuribourb = GetComponent<Rigidbody2D>();
        kuribouAnim = GetComponent<Animator>();
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned in the inspector.");
        }
    }

    void Update()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            hasTouchedGround = true; // GroundTagに接触したら、左右の移動を開始する
        }

        if (collision.collider.CompareTag(turnTag) || collision.collider.CompareTag(kuribouTag))
        {
            turn++;
            Debug.Log("Kuribou turned");
        }

        if (collision.collider.CompareTag(death))
        {
            Destroy(this.gameObject);
        }
    }
}
