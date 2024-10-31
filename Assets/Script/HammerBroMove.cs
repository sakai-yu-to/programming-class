using UnityEngine;

public class HammerBroMove : MonoBehaviour
{
    // 移動関連のパラメータ
    public Vector2 moveSpeed = new Vector2(8f, 15f);
    public float moveChangeInterval = 1f;
    public float jumpForce = 10f;
    public float ySpeed = -7f;
    public Vector2 jumpIntervalRange = new Vector2(1f, 3f);
    public float jumpHeightLimit;
    public Vector2 jumpHeightRange = new Vector2(1f, 3f);

    // ハンマー投げ関連のパラメータ
    public GameObject hammerPrefab; // ハンマープレハブ
    public Transform hammerSpawnPoint; // ハンマーの発射位置
    public float throwInterval = 2f; // ハンマーを投げる間隔
    public Vector2 throwForceRange = new Vector2(3f, 7f); // ランダムな投げる力の範囲
    public Vector2 throwDirection = new Vector2(1f, 1f); // ハンマーの投げる方向

    private Rigidbody2D rb;
    private Transform player;
    private float currentMoveSpeed;
    private float moveChangeTimer;
    private float jumpTimer;
    private float nextJumpTime;
    private int moveDirection = 1;
    private float initialJumpY;
    private bool isJumping = false;
    private float throwTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 初回のランダムな移動速度とジャンプ時間の設定
        currentMoveSpeed = Random.Range(moveSpeed.x, moveSpeed.y);
        nextJumpTime = Random.Range(jumpIntervalRange.x, jumpIntervalRange.y);
        jumpHeightLimit = Random.Range(jumpHeightRange.x, jumpHeightRange.y);
        moveChangeInterval = Random.Range(1f, 2f);
        throwInterval = Random.Range(1f, 3f);
    }

    private void Update()
    {
        // 移動処理
        HandleMovement();

        // ハンマー投げ処理
        HandleHammerThrow();
    }

    private void HandleMovement()
    {
        // 一定時間ごとに移動方向と速度を変更
        moveChangeTimer += Time.deltaTime;
        if (moveChangeTimer >= moveChangeInterval)
        {
            currentMoveSpeed = Random.Range(moveSpeed.x, moveSpeed.y);
            moveDirection *= -1;
            moveChangeTimer = 0f;
            moveChangeInterval = Random.Range(1f, 2f);
        }

        // プレイヤーの方向に向く
        Vector3 direction = player.position - transform.position;
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1.3f, 2.6f, 1.3f); // 左を向く
        }
        else
        {
            transform.localScale = new Vector3(-1.3f, 2.6f, 1.3f); // 右を向く
        }

        // X軸の移動速度を設定
        float xSpeed = currentMoveSpeed * moveDirection;

        // ジャンプ処理
        jumpTimer += Time.deltaTime;
        if (!isJumping && jumpTimer >= nextJumpTime)
        {
            initialJumpY = transform.position.y;
            isJumping = true;
            ySpeed = jumpForce;
        }
        else if (isJumping && transform.position.y - initialJumpY >= jumpHeightLimit)
        {
            isJumping = false;
            ySpeed = -7;
            nextJumpTime = Random.Range(jumpIntervalRange.x, jumpIntervalRange.y);
            jumpTimer = 0f;
            jumpHeightLimit = Random.Range(jumpHeightRange.x, jumpHeightRange.y);
        }

        // 常にX軸の移動速度を設定 (ジャンプ中でも移動する)
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    private void HandleHammerThrow()
    {
        // 一定時間ごとにハンマーを投げる
        throwTimer += Time.deltaTime;
        if (throwTimer >= throwInterval)
        {
            ThrowHammer();
            throwTimer = 0f;
            throwInterval = Random.Range(1f, 3f);
        }
    }

    private void ThrowHammer()
    {
        // ハンマーを生成
        GameObject hammer = Instantiate(hammerPrefab, hammerSpawnPoint.position, Quaternion.identity);

        // プレイヤーの方向に応じてハンマーの投げる方向を決定
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 finalThrowDirection = new Vector2(throwDirection.x * directionToPlayer.x, throwDirection.y);

        // ランダムな投げる力を決定
        float randomThrowForce = Random.Range(throwForceRange.x, throwForceRange.y);

        // ハンマーに力を加えて放物線を描かせる
        Rigidbody2D rbHammer = hammer.GetComponent<Rigidbody2D>();
        rbHammer.AddForce(finalThrowDirection * randomThrowForce, ForceMode2D.Impulse);

    }
}
