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
    public float bounceHeight; // �o�E���X���̍ő卂��
    public float bounceSpeed; // �o�E���X���̑��x
    public CheckGround ground;
    public CheckGround head;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isHead = false;

    private bool isJump = false;
    private float jumpPos = 0.0f;
    private float bounceStartPos = 0.0f; // �o�E���X�J�n���̍���

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

    private bool enemyBounce = false; // �G�ɓ������ăo�E���h����t���O

    private string goalTag = "Goal";
    private bool goalFlag = false;
    public GameObject goal;
    public GameObject goaled;
    public bool finish = false;

    private float goalMoveTimer = 0.0f; // �S�[����̓���𐧌䂷�邽�߂̃^�C�}�[
    private bool isGoalMoving = false;  // �S�[����̈ړ��𐧌䂷��t���O
    private bool isRandomMoving = false; // �����_���ړ��𐧌䂷��t���O
    private bool isGoalActionComplete = false; // �S�[����̓��삪�����������ǂ����̃t���O



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
        bounceStartPos = transform.position.y; // �o�E���X�J�n���̍������L�^
        ySpeed = bounceSpeed; // �o�E���X�p�̑��x��ݒ�
        isJump = true; // �W�����v�t���O�𗧂Ă�
        enemyBounce = true; // �G����̃o�E���h�t���O�𗧂Ă�
        rb.velocity = new Vector2(xSpeed, ySpeed); // ���x���X�V
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
