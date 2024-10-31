using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMove : MonoBehaviour
{
    public bool moveVertically = false;   // �c�����̈ړ��t���O
    public bool moveHorizontally = false; // �������̈ړ��t���O
    public float rotationSpeed = 50f;     // ��]���x
    public float directionChangeInterval = 2f; // �ړ������𔽓]����Ԋu�i�b�j

    private Rigidbody2D rb;
    private float timer;
    private Vector2 direction;

    public changeValue changevalue;

    void Start()
    {
        GameObject gamemanager = GameObject.Find("Gamemanager");
        if (gamemanager != null)
        {
            changevalue = gamemanager.GetComponent<changeValue>();
        }

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D �R���|�[�l���g���擾
        direction = Vector2.one;          // �����ړ�������1�i���̕����j�ɐݒ�

        if (!Gamemanager.instance.isHard)
        {
            Destroy(this.gameObject);
        }

    }

    void Update()
    {
        // ���v���̉�]
        rb.angularVelocity = -rotationSpeed;  // ���̒l�Ŏ��v���

        // ��莞�Ԃ��ƂɈړ������𔽓]
        timer += Time.deltaTime;
        if (timer > directionChangeInterval)
        {
            timer = 0f;
            direction *= -1;  // �ړ������𔽓]
        }

        // �ړ�����
        Vector2 movement = Vector2.zero;

        if (moveVertically)
        {
            // �c�����̑��x��ݒ�
            movement.y = direction.y * changevalue.gearmoveSpeed;
        }

        if (moveHorizontally)
        {
            // �������̑��x��ݒ�
            movement.x = direction.x * changevalue.gearmoveSpeed;
        }

        // Rigidbody2D �̑��x��ݒ�
        rb.velocity = movement;
    }
}
