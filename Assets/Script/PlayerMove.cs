using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using System.Xml.Serialization;

public class PlayerMove : MonoBehaviour
{
    public GameObject hardImage;

    public float bounceHeight;
    public float killKuribouHeight = 1.0f;
    public float killKillerHeight = 4.0f;

    public float bounceSpeed; 
    public CheckGround ground;
    public CheckGround head;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    public bool isGround = false;
    private bool isHead = false;

    public bool isJump = false;
    private float jumpPos = 0.0f;
    private float bounceStartPos = 0.0f; 

    public float xSpeed = 0.0f;
    public float ySpeed = 0.0f;


    public float down_time;
    private bool down_flag = false;
    private bool isDown = false;

    public Tilemap tilemap;
    public TileBase coinedBlockTile;

    public AudioSource coinSource;
    public AudioSource damageSource;
    public AudioSource goalSource;
    public AudioSource gameOverSource;
    public AudioSource dokanSource;
    public AudioSource jumpSource;


    public AudioClip coinSound;
    public AudioClip stage1Music;
    public AudioClip damageSound;
    public AudioClip goalSound;
    public AudioClip gameOverSound;
    public AudioClip dokanSound;
    public AudioClip jumpSound;

    public bool getCoin = false;
    public bool damageFlag = false;
    public GameObject damagePanel;
    private float damagedTime = 0.0f;
    private bool damageRed = false;


    public bool enemyBounce = false; 

    private bool goalFlag = false;
    public GameObject goal;
    public GameObject goaled;
    public bool finish = false;

    private float goalMoveTimer = 0.0f; // ゴール後の動作を制御するためのタイマー
    private bool isGoalMoving = false;  // ゴール後の移動を制御するフラグ
    private bool isRandomMoving = false; // ランダム移動を制御するフラグ
    private bool isGoalActionComplete = false; // ゴール後の動作が完了したかどうかのフラグ

    private float jumpingTime = 0.0f;
    private float moveMenuTime = 0.0f;

    public float invincibilityTime = 2.0f;
    private float lastDamageTime;
    private bool invincible = false;
    public bool timerStop = false;
    private bool gameoverFlag = false;

    private Vector3 originalPosition;

    private bool moveStartFlag = false;
    private float moveStartTime = 0.0f;

    private bool jumpFlag = false;
    private bool dokanSoundFlag = false;

    public  bool hitDossunFlag = false;

    public bool killKuribou = false;
    public bool killKiller = false;

    private SpriteRenderer spriteRenderer;

    public changeValue changevalue;
    void Start()
    {
        GameObject gamemanager = GameObject.Find("Gamemanager");
        if (gamemanager != null)
        {
            changevalue = gamemanager.GetComponent<changeValue>();
        }
        Gamemanager.instance.life = 10;
        Gamemanager.instance.totalCoin = 0;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        down_time = 0;
        damagePanel.SetActive(false);
        goaled.SetActive(false);
        originalPosition = transform.position;
        Gamemanager.instance.startBgm = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (Gamemanager.instance.isHard)
        {
            hardImage.SetActive(true);
        }
        else
        {
            hardImage.SetActive(false);
        }

    }

    void Update()
    {
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);

        if (Input.GetKey(KeyCode.R))
        {
            isDown = true;
            down_flag = true;
            Gamemanager.instance.playStage = 0;
            SceneManager.LoadScene("Menu");
        }


        if (Gamemanager.instance.life == 0 && !gameoverFlag)
        {
            transform.localScale = new Vector3(1.3f, -1.3f, 1.3f);
            isDown = true;
            down_flag = true;
            Gamemanager.instance.stageSource.Stop();
            gameOverSource.Play();
            gameoverFlag = true;
            Collider2D col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        if (down_time >= 2.5f)
        {
            Gamemanager.instance.playStage = 0;
            SceneManager.LoadScene("Menu");
        }



        if (ySpeed >= changevalue.playerJumpSpeed * 3)
        {
            jumpingTime += Time.deltaTime;
            if(jumpingTime > 0.5f)
            {
                jumpingTime = 0.0f;
                ySpeed = -changevalue.playerGravity;
            }
        }

        // ゴール後の動作を制御
        if (goalFlag)
        {
            goalMoveTimer += Time.deltaTime; // タイマーを更新

            if (goalMoveTimer < 1.0f) // 最初の1秒は待機 (ワープ前)
            {
                rb.velocity = new Vector2(0, -changevalue.playerGravity);
            }
            else if (goalMoveTimer >= 1.0f && goalMoveTimer < 1.5f) // 次の1秒は右に移動 (ワープ後)
            {
                if (!isGoalMoving) // 初回のみワープ処理を行う
                {
                    Debug.Log("warp prepare");
                    // ゴール時にプレイヤーを右に1単位ワープさせる
                    Vector3 newPosition = transform.position;
                    newPosition.x += 1.0f; // x座標を1増加
                    transform.position = newPosition; // 新しい位置にワープさせる

                    isGoalMoving = true; // 移動を開始する
                }
                rb.velocity = new Vector2(changevalue.playerSpeed, rb.velocity.y); // 右方向に移動
                Debug.Log("go right");
            }
            else if (goalMoveTimer >= 1.5f && goalMoveTimer < 4.0f) // 次の1秒はランダムに動く
            {
                if (isGoalMoving) // ランダム移動の開始
                {
                    isGoalMoving = false; // 右移動終了
                    isRandomMoving = true; // ランダム移動開始
                }
                Debug.Log("random moving");
                // ランダムな方向に移動
                float randomX = Random.Range(-2f, 2f); // -1から1の範囲でランダムなX方向を選択
                float randomY = Random.Range(-1f, 2f); // -1から1の範囲でランダムなY方向を選択
                rb.velocity = new Vector2(randomX * changevalue.playerSpeed, randomY * changevalue.playerSpeed);
            }
            else // すべての動作終了
            {
                Debug.Log("goal finished");
                isRandomMoving = false; // ランダム動作終了
                rb.velocity = Vector2.zero; // 停止
                isGoalActionComplete = true; // ゴール後のすべての動作が終了
                goalFlag = false; // ゴール処理終了
            }

            // ゴール後の処理が行われている間は、これ以上の処理を行わない
            return;
        }

        if (isGoalActionComplete)
        {
            moveMenuTime += Time.deltaTime;
            if(moveMenuTime > 2.0f)
            {
                SceneManager.LoadScene("Score");

            }
        }

        // ゴール後の処理が行われていない場合の通常の移動処理
        if (!isDown && !finish)
        {
            isGround = ground.IsGround();
            isHead = head.IsGround();

            // 地面にいるか、または敵を倒した場合はジャンプを許可
            if (isGround)
            {
                if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.Space)))
                {
                    isJump = true;
                    ySpeed = changevalue.playerJumpSpeed * 3;
                    jumpPos = transform.position.y;


                }
                else
                {
                    isJump = false;
                }
            }
            else if (enemyBounce) // バウンス中の処理
            {
                if ((bounceStartPos + bounceHeight) > transform.position.y && !isHead)
                {
                    ySpeed = bounceSpeed;
                }
                else
                {
                    enemyBounce = false; // バウンス終了
                    isJump = false;
                    ySpeed = -changevalue.playerGravity; // 重力で落下する
                }
            }
            else if (isJump)
            {
                isGround = false;
                if (((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.Space))) && (jumpPos + changevalue.playerJumpHeight) > transform.position.y && !isHead)
                {
                    ySpeed = changevalue.playerJumpSpeed;
                }
                else
                {
                    isJump = false;
                }
            }
            else
            {
                ySpeed = -changevalue.playerGravity;
            }

            if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                anim.SetBool("run", true);
                xSpeed = changevalue.playerSpeed;
            }
            else if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            {
                transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
                anim.SetBool("run", true);
                xSpeed = -changevalue.playerSpeed;
              }
            else
            {
                anim.SetBool("run", false);
                xSpeed = 0.0f;
            }

            if (hitDossunFlag && !isGround)
            {
                xSpeed = 0.0f;
                ySpeed = -changevalue.playerGravity;
            }


            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, -changevalue.playerGravity);
        }

        if (transform.position.y > 11)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
            rb.velocity = new Vector2(0, -changevalue.playerGravity);
            enemyBounce = false;
        }


        if (isGround && hitDossunFlag)
        {
            hitDossunFlag = false;
        }

        if (down_flag)
        {
            down_time += Time.deltaTime;
        }

        if (getCoin)
        {
            Gamemanager.instance.totalCoin++;
            coinSource.PlayOneShot(coinSound);
            getCoin = false;
        }

        if(damageFlag && invincible)
        {
            damageFlag = false;
        }
        if (damageFlag && !invincible && Gamemanager.instance.life > 0)
        {

            Gamemanager.instance.life--;
            damageSource.PlayOneShot(damageSound);
            damagePanel.SetActive(true);
            damageRed = true;
            damageFlag = false;

            invincible = true;
            lastDamageTime = Time.time;
        }

        // 無敵時間の解除
        if (invincible)
        {
            float timeSinceDamage = Time.time - lastDamageTime;
            if (timeSinceDamage > invincibilityTime)
            {
                invincible = false;  // 無敵解除
                spriteRenderer.enabled = true;  // 最後に点滅を解除し、表示を元に戻す
            }
            else
            {
                // 点滅処理：タイムスケールを利用してスプライトを表示・非表示に切り替え
                spriteRenderer.enabled = Mathf.FloorToInt(timeSinceDamage * 10) % 2 == 0;
            }
        }

        if (damageRed)
        {
            damagedTime += Time.deltaTime;
            if (damagedTime > 0.3f)
            {
                damagePanel.SetActive(false);
                damagedTime = 0.0f;
                damageRed = false;
            }
        }

        if (moveStartFlag)
        {
            isDown = true;
            moveStartTime += Time.deltaTime;
            rb.velocity = new Vector2(2, 0);
            if (moveStartTime > 2.0f)
            {
                dokanSoundFlag = false;
                isDown = false;
                moveStartTime = 0.0f;
                moveStartFlag = false;
                transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
            }

        }

        if (ySpeed > 0 && !jumpFlag)
        {
            jumpSource.PlayOneShot(jumpSound);
            jumpFlag = true;
        }
        else if(ySpeed < 0)
        {
            jumpFlag = false;
        }


    }

    public void BounceOnEnemy()
    {
        Debug.Log("jump by kill");

        if (killKuribou)
        {
            bounceHeight = killKuribouHeight;
            killKuribou = false;
        }

        if (killKiller)
        {
            bounceHeight = killKillerHeight; 
            killKiller = false;
        }

        bounceStartPos = transform.position.y; // バウンス開始時の高さを記録
        ySpeed = bounceSpeed; // バウンス用の速度を設定
        isJump = true; // ジャンプフラグを立てる
        enemyBounce = true; // 敵からのバウンドフラグを立てる
        rb.velocity = new Vector2(xSpeed, ySpeed); // 速度を更新
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Goal"))
        {
            timerStop = true;
            Gamemanager.instance.stageSource.Stop();
            goalSource.PlayOneShot(goalSound);
            goal.SetActive(false);
            goaled.SetActive(true);
            finish = true;
            goalFlag = true;
        }

        if(!finish)
        {
            if (collision.collider.CompareTag("Dead"))
            {
                Gamemanager.instance.life = 0;
            }

            if (collision.collider.CompareTag("Kuribou"))
            {
                Debug.Log("Player hit kuribou!");
                damageFlag = true;
            }

            if (collision.collider.CompareTag("HammerBro"))
            {
                Debug.Log("Player hit HammerBro!");
                damageFlag = true;
            }


            if (collision.collider.CompareTag("Dossun"))
            {
                Debug.Log("Player hit dossun!");
                damageFlag = true;
            }


            if (collision.collider.CompareTag("Killer"))
            {
                Debug.Log("Player hit killer!");
                isJump = false;
                damageFlag = true;
            }


            if (collision.collider.CompareTag("Pakkun"))
            {
                Debug.Log("Player hit pakkun!");
                damageFlag = true;
            }

            if (collision.collider.CompareTag("Boss"))
            {
                Debug.Log("Player hit Boss!");
                damageFlag = true;
            }


            if (collision.collider.CompareTag("Coin"))
            {
                collision.gameObject.SetActive(false);
                getCoin = true;
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.collider.CompareTag("moveStage1") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            SceneManager.LoadScene("Stage1");
        }

        if (collision.collider.CompareTag("moveStage2") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            SceneManager.LoadScene("Stage2");
        }

        if (collision.collider.CompareTag("moveStage3") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            SceneManager.LoadScene("Stage3");
        }
        /*
        if (collision.collider.CompareTag("moveStart") && ( Input.GetKey(KeyCode.D)  || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
        }
        */
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("killer") && !killKiller)
        {
            Debug.Log("Player hit killer!");
            damageFlag = true;
        }
    }
    */

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("moveBossStage") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {

            SceneManager.LoadScene("BossStage");
        }

        if(!finish)
        {
            if (other.gameObject.CompareTag("moveStart") && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !dokanSoundFlag)
            {
                dokanSource.Play();
                moveStartFlag = true;
            }

            if (other.gameObject.CompareTag("Fire"))
            {
                Debug.Log("Player hit Fire!");
                damageFlag = true;
            }
        }

    }

}
