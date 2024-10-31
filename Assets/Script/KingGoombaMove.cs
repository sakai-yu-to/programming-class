using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingGoombaMove : MonoBehaviour
{
    public GameObject player;
    private float kuribouxSpeed;
    private Animator kuribouAnim = null;
    private Rigidbody2D kuribourb = null;
    private bool hasTouchedGround = false; // �n�ʂɐڐG�������ǂ������L�^����t���O
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
            // �n�ʂɐڐG������̍��E�̈ړ�����
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
            // �n�ʂɐڐG����܂ł͐^���ɗ�������
            kuribourb.velocity = new Vector2(0, kuribourb.velocity.y);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            hasTouchedGround = true; // "Ground"�ɐڐG������A���E�̈ړ����J�n����
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
