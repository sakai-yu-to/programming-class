using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossunMove : MonoBehaviour
{
    public GameObject player; // �v���C���[�̎Q��
    public float fallSpeed = 5f; // �������x
    public float riseSpeed = 2f; // �㏸���x
    public float triggerDistance; // �v���C���[���߂Â�����
    private Vector3 originalPosition; // �h�b�X���̌��̈ʒu
    private bool isFalling = false; // �h�b�X�������������ǂ���
    private bool isRising = false; // �h�b�X�����㏸�����ǂ���
    private bool isWaiting = false; // �ҋ@�����ǂ���



    private Rigidbody2D rb;

    void Start()
    {
        originalPosition = transform.position; // ���̈ʒu��ۑ�
        rb = GetComponent<Rigidbody2D>();

        // Z���̉�]�����b�N���ăh�b�X�����X���Ȃ��悤�ɂ���
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player = GameObject.Find("Player");  // �v���C���[��T��

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

            // �v���C���[���߂Â����痎�����J�n
            if ((distanceToPlayerX <= triggerDistance) && distanceToPlayerY > 0)
            {
                isFalling = true;
            }
        }

        if (isFalling)
        {
            Fall();
        }
        else if (isRising)
        {
            Rise();
        }


    }

    void Fall()
    {
        // �h�b�X���𗎉�������iRigidbody2D���g�����Ƃŕ����I�ȗ����������j
        rb.velocity = Vector2.down * fallSpeed;
    }

    void Rise()
    {
        // �h�b�X��������̍����܂ŏ㏸������
        rb.velocity = Vector2.up * riseSpeed;

        if (transform.position.y >= originalPosition.y)
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
            isRising = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �n�ʂɐG�ꂽ�痎�����~���A2�b�ҋ@��ɏ㏸���J�n
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
