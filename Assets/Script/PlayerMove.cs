using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using System.Xml.Serialization;

public class PlayerMove : MonoBehaviour
{
    public float jumpSpeed;
    public float gravity;
    public float jumpHeight;
    public float bounceHeight;
    public float killKuribouHeight = 1.0f;
    public float killKillerHeight = 4.0f;

    public float bounceSpeed; 
    public CheckGround ground;
    public CheckGround head;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isHead = false;

    private bool isJump = false;
    private float jumpPos = 0.0f;
    private float bounceStartPos = 0.0f; 

    public float speed = 5.0f;
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


    private bool enemyBounce = false; 

    private bool goalFlag = false;
    public GameObject goal;
    public GameObject goaled;
    public bool finish = false;

    private float goalMoveTimer = 0.0f; // �S�[����̓���𐧌䂷�邽�߂̃^�C�}�[
    private bool isGoalMoving = false;  // �S�[����̈ړ��𐧌䂷��t���O
    private bool isRandomMoving = false; // �����_���ړ��𐧌䂷��t���O
    private bool isGoalActionComplete = false; // �S�[����̓��삪�����������ǂ����̃t���O

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

    void Start()
    {
        Gamemanager.instance.life = 10;
        Gamemanager.instance.totalCoin = 0;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        down_time = 0;
        damagePanel.SetActive(false);
        goaled.SetActive(false);
        originalPosition = transform.position;
        Gamemanager.instance.startBgm = true;

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



        if (ySpeed >= jumpSpeed * 3)
        {
            jumpingTime += Time.deltaTime;
            if(jumpingTime > 0.5f)
            {
                jumpingTime = 0.0f;
                ySpeed = -gravity;
            }
        }

        // �S�[����̓���𐧌�
        if (goalFlag)
        {
            goalMoveTimer += Time.deltaTime; // �^�C�}�[���X�V

            if (goalMoveTimer < 1.0f) // �ŏ���1�b�͑ҋ@ (���[�v�O)
            {
                rb.velocity = new Vector2(0, -gravity);
            }
            else if (goalMoveTimer >= 1.0f && goalMoveTimer < 1.5f) // ����1�b�͉E�Ɉړ� (���[�v��)
            {
                if (!isGoalMoving) // ����̂݃��[�v�������s��
                {
                    Debug.Log("warp prepare");
                    // �S�[�����Ƀv���C���[���E��1�P�ʃ��[�v������
                    Vector3 newPosition = transform.position;
                    newPosition.x += 1.0f; // x���W��1����
                    transform.position = newPosition; // �V�����ʒu�Ƀ��[�v������

                    isGoalMoving = true; // �ړ����J�n����
                }
                rb.velocity = new Vector2(speed, rb.velocity.y); // �E�����Ɉړ�
                Debug.Log("go right");
            }
            else if (goalMoveTimer >= 1.5f && goalMoveTimer < 4.0f) // ����1�b�̓����_���ɓ���
            {
                if (isGoalMoving) // �����_���ړ��̊J�n
                {
                    isGoalMoving = false; // �E�ړ��I��
                    isRandomMoving = true; // �����_���ړ��J�n
                }
                Debug.Log("random moving");
                // �����_���ȕ����Ɉړ�
                float randomX = Random.Range(-2f, 2f); // -1����1�͈̔͂Ń����_����X������I��
                float randomY = Random.Range(-1f, 2f); // -1����1�͈̔͂Ń����_����Y������I��
                rb.velocity = new Vector2(randomX * speed, randomY * speed);
            }
            else // ���ׂĂ̓���I��
            {
                Debug.Log("goal finished");
                isRandomMoving = false; // �����_������I��
                rb.velocity = Vector2.zero; // ��~
                isGoalActionComplete = true; // �S�[����̂��ׂĂ̓��삪�I��
                goalFlag = false; // �S�[�������I��
            }

            // �S�[����̏������s���Ă���Ԃ́A����ȏ�̏������s��Ȃ�
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

        // �S�[����̏������s���Ă��Ȃ��ꍇ�̒ʏ�̈ړ�����
        if (!isDown && !finish)
        {
            isGround = ground.IsGround();
            isHead = head.IsGround();

            // �n�ʂɂ��邩�A�܂��͓G��|�����ꍇ�̓W�����v������
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
            else if (enemyBounce) // �o�E���X���̏���
            {
                if ((bounceStartPos + bounceHeight) > transform.position.y && !isHead)
                {
                    ySpeed = bounceSpeed;
                }
                else
                {
                    enemyBounce = false; // �o�E���X�I��
                    isJump = false;
                    ySpeed = -gravity; // �d�͂ŗ�������
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

            if (hitDossunFlag && !isGround)
            {
                xSpeed = 0.0f;
                ySpeed = -gravity;
            }


            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
        }

        if (transform.position.y > 11)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
            rb.velocity = new Vector2(0, -gravity);
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
        if (damageFlag && !invincible)
        {

            Gamemanager.instance.life--;
            damageSource.PlayOneShot(damageSound);
            damagePanel.SetActive(true);
            damageRed = true;
            damageFlag = false;

            invincible = true;
            lastDamageTime = Time.time;
        }

        // ���G���Ԃ̉���
        if (invincible && Time.time - lastDamageTime > invincibilityTime)
        {
            invincible = false;  // ���G����
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

        bounceStartPos = transform.position.y; // �o�E���X�J�n���̍������L�^
        ySpeed = bounceSpeed; // �o�E���X�p�̑��x��ݒ�
        isJump = true; // �W�����v�t���O�𗧂Ă�
        enemyBounce = true; // �G����̃o�E���h�t���O�𗧂Ă�
        rb.velocity = new Vector2(xSpeed, ySpeed); // ���x���X�V
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

        if (collision.collider.CompareTag("Dead"))
        {
            Gamemanager.instance.life = 0;
        }

        if (collision.collider.CompareTag("Kuribou"))
        {
            Debug.Log("Player hit kuribou!");
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


        if (collision.collider.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            getCoin = true;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.collider.CompareTag("moveStage1") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            Gamemanager.instance.playStage = 1;
            SceneManager.LoadScene("Stage1");
        }

        if (collision.collider.CompareTag("moveStage2") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            Gamemanager.instance.playStage = 2;
            SceneManager.LoadScene("Stage2");
        }

        if (collision.collider.CompareTag("moveStage3") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            Gamemanager.instance.playStage = 3;
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

            Gamemanager.instance.playStage = 4;
            SceneManager.LoadScene("BossStage");
        }

        if(other.gameObject.CompareTag("moveStart") && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !dokanSoundFlag)
        {
            dokanSource.Play();
            moveStartFlag = true;
        }
    }

}
