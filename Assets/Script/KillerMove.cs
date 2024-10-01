using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillerMove : MonoBehaviour
{
    public GameObject playermove;  // �v���C���[���w��
    public float killerSpeed;  // �G�̈ړ��X�s�[�h
    private Rigidbody2D killerrb;
    private Animator killerAnim;

    public bool isGold = false;  // �v���C���[��ǔ����邩
    public bool goRight = false;  // �E�Ɉړ����邩

    public float checkCycle;  // �ǔ��`�F�b�N����
    private float trackingTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        killerrb = GetComponent<Rigidbody2D>();
        killerAnim = GetComponent<Animator>();
        playermove = GameObject.Find("Player");  // �v���C���[��T��

        // ���������̐ݒ�
        UpdateScaleAndVelocity();  // �����̃X�P�[���Ƒ��x���X�V
    }

    // Update is called once per frame
    void Update()
    {
        if (isGold)
        {
            // �v���C���[��ǔ����郍�W�b�N
            trackingTime += Time.deltaTime;
            if (trackingTime >= checkCycle)
            {
                trackingTime = 0.0f;

                // �v���C���[�̕������v�Z
                Vector2 direction = (playermove.transform.position - transform.position).normalized;
                killerrb.velocity = direction * killerSpeed;

                // �X�v���C�g�̌�����ݒ�
                UpdateScaleAndRotation(direction);
            }
        }
        else
        {
            // goRight��true�Ȃ�E�Afalse�Ȃ獶�Ɉړ�
            UpdateScaleAndVelocity();  // �X�P�[���Ƒ��x���X�V
        }
    }

    private void UpdateScaleAndVelocity()
    {
        Vector3 localScale = transform.localScale;
        if (goRight)
        {
            localScale.x = -0.15f;  // �E�����ɃX�P�[��
            killerrb.velocity = new Vector2(killerSpeed, 0);  // �E�Ɉړ�
        }
        else
        {
            localScale.x = 0.15f;  // �������ɃX�P�[��
            killerrb.velocity = new Vector2(-killerSpeed, 0);  // ���Ɉړ�
        }
        transform.localScale = localScale;
    }

    private void UpdateScaleAndRotation(Vector2 direction)
    {
        // �X�v���C�g�̌�����ݒ�
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-0.15f, 0.15f, 1);  // �E�����ɃX�P�[��
        }
        else
        {
            transform.localScale = new Vector3(0.15f, 0.15f, 1);  // �������ɃX�P�[��]
            direction.y *= -1;
            direction.x *= -1;
        }

        // �v���C���[�Ɍ������ĉ�]
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // ���W�A����x�ɕϊ�
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  // ��]��ݒ�

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Dead"))
        {
            Destroy(this.gameObject);  // "Dead"�^�O�̃I�u�W�F�N�g�ɏՓ˂����玩�g��j��
        }
    }
}
