using UnityEngine;

public class PatapataMovement : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform
    public float activationDistance = 5f; // �v���C���[������ȏ�߂Â����瓮���n�߂鋗��
    public float speed = 2f; // �ړ����x
    public float height = 2f; // �㉺�̈ړ��͈�
    public Sprite upwardSprite; // ������ړ����̃X�v���C�g
    public Sprite downwardSprite; // �������ړ����̃X�v���C�g

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool movingUp = true;
    private bool isActivated = false; // �����n�߂����ǂ����̃t���O

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D�R���|�[�l���g���擾
        startPos = transform.position; // �����ʒu��ۑ�
        rb.gravityScale = 0; // �d�͂𖳌���
    }

    void Update()
    {
        // �v���C���[�Ƃ̋����𑪒�
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ��苗���ȓ��Ƀv���C���[���߂Â�����㉺�^�����J�n
        if (distanceToPlayer <= activationDistance)
        {
            isActivated = true;
        }

        // �㉺�^��������
        if (isActivated)
        {
            if (movingUp)
            {
                rb.velocity = new Vector2(0, speed); // ������Ɉړ�
                if (transform.position.y >= startPos.y + height)
                {
                    movingUp = false;
                }
            }
            else
            {
                rb.velocity = new Vector2(0, -speed); // �������Ɉړ�
                if (transform.position.y <= startPos.y - height)
                {
                    movingUp = true;
                }
            }

            // �ړ������ɉ����ăX�v���C�g��ύX
            if (rb.velocity.y > 0)
            {
                spriteRenderer.sprite = upwardSprite; // ������ړ����̃X�v���C�g�ɕύX
            }
            else if (rb.velocity.y < 0)
            {
                spriteRenderer.sprite = downwardSprite; // �������ړ����̃X�v���C�g�ɕύX
            }
        }
    }
}
