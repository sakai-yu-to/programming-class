using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMove : MonoBehaviour
{
    public GameObject player;
    public float fallSpeed = 5f; // �������x
    public float riseSpeed = 2f; // �㏸���x
    public float triggerDistance; // �v���C���[���߂Â�����
    public float waitTime = 2f; // �ҋ@����
    public float riseHeight = 5f; // �㏸����Y���W�̍�

    private bool isFalling = false; // ���������ǂ���
    private bool isRising = false; // �㏸�����ǂ���
    private bool isWaiting = false; // �ҋ@�����ǂ���

    private Rigidbody2D rb;
    private float originalY; // ����Y���W
    private float waitTimer = 0f; // �ҋ@�^�C�}�[

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        originalY = transform.position.y; // ����Y���W��ێ�
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFalling && !isRising && !isWaiting)
        {
            float distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
            if (distanceToPlayerX <= triggerDistance)
            {
                isRising = true; // �v���C���[���߂Â��Ƃ܂��㏸
            }
        }

        if (isRising)
        {
            rb.velocity = Vector2.up * riseSpeed;
            transform.localScale = new Vector3(0.25f, -0.25f, 0.25f);
            // �w��̍����ɒB�����痎���J�n
            if (transform.position.y >= originalY + riseHeight)
            {
                isRising = false;
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            rb.velocity = Vector2.down * fallSpeed;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            // ����Y���W�܂Ŗ߂������~���đҋ@��Ԃֈڍs
            if (transform.position.y <= originalY)
            {
                transform.position = new Vector2(transform.position.x, originalY); // ���W�𐳊m�Ɍ��ɖ߂�
                rb.velocity = Vector2.zero; // ���x���[���ɂ��Ē�~
                isFalling = false;
                isWaiting = true;
                waitTimer = waitTime; // �ҋ@���Ԃ��Z�b�g
            }
        }
        else if (isWaiting)
        {
            waitTimer -= Time.deltaTime; // �^�C�}�[�����炷

            // ��莞�ԑҋ@������Ăуv���C���[�ɔ�������
            if (waitTimer <= 0f)
            {
                isWaiting = false; // �ҋ@�I����A�Ăуv���C���[�ɔ��������Ԃɖ߂�
            }
        }
    }
}
