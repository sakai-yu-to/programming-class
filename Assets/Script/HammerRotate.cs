using System.Xml.Serialization;
using UnityEngine;

public class HammerRotate : MonoBehaviour
{
    public float rotationSpeed = 360f; // 回転速度 (1秒間に360度回転)
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // ハンマーを常に回転させる
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
