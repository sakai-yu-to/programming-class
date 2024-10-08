using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarMove : MonoBehaviour
{
    public float rotationSpeed = 100f; // ��]���x�i�C���X�y�N�^�[�ŕύX�\�j
    public bool isClockwise = false;   // true�Ȃ玞�v���Afalse�Ȃ甼���v���

    // Update is called once per frame
    void Update()
    {
        // ��]������؂�ւ���
        float direction = isClockwise ? -1f : 1f;

        // ��]�̎��s
        transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}
