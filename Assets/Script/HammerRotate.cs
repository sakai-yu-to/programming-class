using System.Xml.Serialization;
using UnityEngine;

public class HammerRotate : MonoBehaviour
{
    public float rotationSpeed = 360f; // ��]���x (1�b�Ԃ�360�x��])
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // �n���}�[����ɉ�]������
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dead") || other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }


}
