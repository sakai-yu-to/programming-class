using UnityEngine;

public class HammerBroMove : MonoBehaviour
{
    // �ړ��֘A�̃p�����[�^
    public Vector2 moveSpeed = new Vector2(8f, 15f);
    public float moveChangeInterval = 1f;
    public float jumpForce = 10f;
    public float ySpeed = -7f;
    public Vector2 jumpIntervalRange = new Vector2(1f, 3f);
    public float jumpHeightLimit;
    public Vector2 jumpHeightRange = new Vector2(1f, 3f);

    // �n���}�[�����֘A�̃p�����[�^
    public GameObject hammerPrefab; // �n���}�[�v���n�u
    public Transform hammerSpawnPoint; // �n���}�[�̔��ˈʒu
    public float throwInterval = 2f; // �n���}�[�𓊂���Ԋu
    public Vector2 throwForceRange = new Vector2(3f, 7f); // �����_���ȓ�����͈͂̔�
    public Vector2 throwDirection = new Vector2(1f, 1f); // �n���}�[�̓��������

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

        // ����̃����_���Ȉړ����x�ƃW�����v���Ԃ̐ݒ�
        currentMoveSpeed = Random.Range(moveSpeed.x, moveSpeed.y);
        nextJumpTime = Random.Range(jumpIntervalRange.x, jumpIntervalRange.y);
        jumpHeightLimit = Random.Range(jumpHeightRange.x, jumpHeightRange.y);
        moveChangeInterval = Random.Range(1f, 2f);
        throwInterval = Random.Range(1f, 3f);
    }

    private void Update()
    {
        // �ړ�����
        HandleMovement();

        // �n���}�[��������
        HandleHammerThrow();
    }

    private void HandleMovement()
    {
        // ��莞�Ԃ��ƂɈړ������Ƒ��x��ύX
        moveChangeTimer += Time.deltaTime;
        if (moveChangeTimer >= moveChangeInterval)
        {
            currentMoveSpeed = Random.Range(moveSpeed.x, moveSpeed.y);
            moveDirection *= -1;
            moveChangeTimer = 0f;
            moveChangeInterval = Random.Range(1f, 2f);
        }

        // �v���C���[�̕����Ɍ���
        Vector3 direction = player.position - transform.position;
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1.3f, 2.6f, 1.3f); // ��������
        }
        else
        {
            transform.localScale = new Vector3(-1.3f, 2.6f, 1.3f); // �E������
        }

        // X���̈ړ����x��ݒ�
        float xSpeed = currentMoveSpeed * moveDirection;

        // �W�����v����
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

        // ���X���̈ړ����x��ݒ� (�W�����v���ł��ړ�����)
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    private void HandleHammerThrow()
    {
        // ��莞�Ԃ��ƂɃn���}�[�𓊂���
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
        // �n���}�[�𐶐�
        GameObject hammer = Instantiate(hammerPrefab, hammerSpawnPoint.position, Quaternion.identity);

        // �v���C���[�̕����ɉ����ăn���}�[�̓��������������
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 finalThrowDirection = new Vector2(throwDirection.x * directionToPlayer.x, throwDirection.y);

        // �����_���ȓ�����͂�����
        float randomThrowForce = Random.Range(throwForceRange.x, throwForceRange.y);

        // �n���}�[�ɗ͂������ĕ�������`������
        Rigidbody2D rbHammer = hammer.GetComponent<Rigidbody2D>();
        rbHammer.AddForce(finalThrowDirection * randomThrowForce, ForceMode2D.Impulse);

    }
}
