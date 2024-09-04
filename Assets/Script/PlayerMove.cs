using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;

public class PlayerMove : MonoBehaviour
{
    public float jumpSpeed;
    public float gravity;
    public float jumpHeight;
    public float bounceHeight; // バウンス時の最大高さ
    public float bounceSpeed; // バウンス時の速度
    public CheckGround ground;
    public CheckGround head;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isHead = false;

    private bool isJump = false;
    private float jumpPos = 0.0f;
    private float bounceStartPos = 0.0f; // バウンス開始時の高さ

    public float speed = 5.0f;
    public float xSpeed = 0.0f;
    public float ySpeed = 0.0f;

    private string headTag = "Head";
    private string footTag = "Foot";

    private string enemyTag = "Enemy";
    private string kuribouTag = "Kuribou";
    private string dossunTag = "Dossun";
    public float down_time;
    private bool down_flag = false;
    private bool isDown = false;

    private string coinTag = "Coin";

    private string coinBlockTag = "CoinBlock";
    private string deadTag = "Dead";

    public Tilemap tilemap;
    public TileBase coinedBlockTile;

    public AudioSource coinSource;
    public AudioSource stage1Source;
    public AudioSource damageSource;
    public AudioSource goalSource;

    public AudioClip coinSound;
    public AudioClip stage1Music;
    public AudioClip damageSound;
    public AudioClip goalSound;

    public bool getCoin = false;
    public bool damageFlag = false;
    public GameObject damagePanel;
    private float damagedTime = 0.0f;
    private bool damageRed = false;

    private bool enemyBounce = false; // 敵に当たってバウンドするフラグ

    private string goalTag = "Goal";
    private bool goalFlag = false;
    public GameObject goal;
    public GameObject goaled;
    public bool finish = false;

    private float goalMoveTimer = 0.0f; // ゴール後の動作を制御するためのタイマー
    private bool isGoalMoving = false;  // ゴール後の移動を制御するフラグ
    private bool isRandomMoving = false; // ランダム移動を制御するフラグ
    private bool isGoalActionComplete = false; // ゴール後の動作が完了したかどうかのフラグ



    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        down_time = 0;
        stage1Source.clip = stage1Music;
        stage1Source.Play();
        damagePanel.SetActive(false);
        goaled.SetActive(false);
    }

    void Update()
    {
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);

        if (Gamemanager.instance.life == 0)
        {
            isDown = true;
            down_flag = true;
            Gamemanager.instance.life = 10;
            SceneManager.LoadScene("Stage1");
        }

        // ゴール後の動作を制御
        if (goalFlag)
        {
            goalMoveTimer += Time.deltaTime; // タイマーを更新

            if (goalMoveTimer < 1.0f) // 最初の1秒は待機 (ワープ前)
            {
                rb.velocity = new Vector2(0, -gravity);
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
                rb.velocity = new Vector2(speed, rb.velocity.y); // 右方向に移動
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
                rb.velocity = new Vector2(randomX * speed, randomY * speed);
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
                    ySpeed = jumpSpeed * 3;
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
                    ySpeed = -gravity; // 重力で落下する
                }
            }
            else if (isJump)
            {
                if (((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.Space))) && (jumpPos + jumpHeight) > transform.position.y && !isHead)
                {
                    ySpeed = jumpSpeed;
                }
                else
                {
                    isJump = false;
                }
            }
            else
            {
                ySpeed = -gravity;
            }

            if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                anim.SetBool("run", true);
                xSpeed = speed;
            }
            else if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            {
                transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
                anim.SetBool("run", true);
                xSpeed = -speed;
            }
            else
            {
                anim.SetBool("run", false);
                xSpeed = 0.0f;
            }

            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
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

        if (damageFlag)
        {
            Gamemanager.instance.life--;
            damageSource.PlayOneShot(damageSound);
            damagePanel.SetActive(true);
            damageRed = true;
            damageFlag = false;
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
    }

    public void BounceOnEnemy()
    {
        Debug.Log("jump by kill");
        bounceStartPos = transform.position.y; // バウンス開始時の高さを記録
        ySpeed = bounceSpeed; // バウンス用の速度を設定
        isJump = true; // ジャンプフラグを立てる
        enemyBounce = true; // 敵からのバウンドフラグを立てる
        rb.velocity = new Vector2(xSpeed, ySpeed); // 速度を更新
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(goalTag))
        {
            stage1Source.Stop();
            goalSource.PlayOneShot(goalSound);
            goal.SetActive(false);
            goaled.SetActive(true);
            finish = true;
            goalFlag = true;
        }

        if (collision.collider.CompareTag(deadTag))
        {
            Gamemanager.instance.life = 0;
        }

        if (collision.collider.CompareTag(kuribouTag))
        {
            Debug.Log("Player hit kuribou!");
            damageFlag = true;
        }

        if (collision.collider.CompareTag(dossunTag))
        {
            Debug.Log("Player hit dossun!");
            damageFlag = true;
        }

        if (collision.collider.CompareTag(coinTag))
        {
            collision.gameObject.SetActive(false);
            getCoin = true;
        }
    }
}
