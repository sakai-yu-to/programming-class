using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossMove : MonoBehaviour
{
    public EnemyKill enemyKill;
    public BossManager bossManager;
    private Rigidbody2D rb;
    private Transform player;
    public float moveSpeedMin = 8f;
    public float moveSpeedMax = 15f;
    public float moveChangeInterval = 1f;
    private float currentMoveSpeed;
    private float moveChangeTimer;
    private int moveDirection = 1;
    public float ySpeed = -7f;
    public float bossScale = 0;
    public float rotationSpeed = 360f; // ‰ñ“]‘¬“x (1•bŠÔ‚É360“x‰ñ“])


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentMoveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.position - transform.position;
        if (bossManager.bossStatus < 2)
        {
            bossScale = 5;
        }
        else if (bossManager.bossStatus <4 )
        {
            bossScale = 7;
        }
        else if(bossManager.bossStatus < 6)
        {
            bossScale = 3;
        }

        if(bossManager.bossStatus == 5)
        {
            Collider2D col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        if(bossManager.bossStatus % 2 == 0)
        {
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-bossScale, bossScale, bossScale); // ¶‚ðŒü‚­
            }
            else
            {
                transform.localScale = new Vector3(bossScale, bossScale, bossScale); // ¶‚ðŒü‚­
            }
        }
        else
        {
            bossScale /= 7;
            transform.localScale = new Vector3(bossScale,bossScale,bossScale);
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        // ˆê’èŽžŠÔ‚²‚Æ‚ÉˆÚ“®•ûŒü‚Æ‘¬“x‚ð•ÏX
        moveChangeTimer += Time.deltaTime;
        if (moveChangeTimer >= moveChangeInterval)
        {
            currentMoveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
            moveDirection *= -1;
            moveChangeTimer = 0f;
            moveChangeInterval = Random.Range(1f, 2f);
        }

        float xSpeed = currentMoveSpeed * moveDirection;

        rb.velocity = new Vector2(xSpeed, ySpeed);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            Destroy(this.gameObject);
        }
    }
}
