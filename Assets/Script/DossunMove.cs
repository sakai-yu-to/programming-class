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
            rb.velocity = Vector2.down * fallSpeed;
        }
        else if (isRising)
        {
            rb.velocity = Vector2.up * riseSpeed;

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
