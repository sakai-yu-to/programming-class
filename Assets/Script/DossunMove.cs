using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossunMove : MonoBehaviour
{
    public GameObject player; // プレイヤーの参照
    public float triggerDistance; // プレイヤーが近づく距離
    private Vector3 originalPosition; // ドッスンの元の位置
    private bool isFalling = false; // ドッスンが落下中かどうか
    private bool isRising = false; // ドッスンが上昇中かどうか
    private bool isWaiting = false; // 待機中かどうか

    private Rigidbody2D rb;

    public changeValue changevalue;

    void Start()
    {
        GameObject gamemanager = GameObject.Find("Gamemanager");
        if (gamemanager != null)
        {
            changevalue = gamemanager.GetComponent<changeValue>();
        }

        originalPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.Find("Player"); 
        if (!Gamemanager.instance.isHard)
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        if (!isFalling && !isRising && !isWaiting)
        {
            float distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
            float distanceToPlayerY = transform.position.y - player.transform.position.y;

            if ((distanceToPlayerX <= triggerDistance) && distanceToPlayerY > 0)
            {
                isFalling = true;
            }
        }

        if (isFalling)
        {
            rb.velocity = Vector2.down * changevalue.dossunFallSpeed;
        }
        else if (isRising)
        {
            rb.velocity = Vector2.up * changevalue.dossunRiseSpeed;

            if (transform.position.y >= originalPosition.y)
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
                isRising = false;
            }
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に触れたら落下を停止し、2秒待機後に上昇を開始
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("breakGround"))
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
