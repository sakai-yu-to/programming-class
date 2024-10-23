using System.Xml.Serialization;
using UnityEngine;

public class HammerRotate : MonoBehaviour
{
    public float rotationSpeed = 360f; // ‰ñ“]‘¬“x (1•bŠÔ‚É360“x‰ñ“])
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // ƒnƒ“ƒ}[‚ğí‚É‰ñ“]‚³‚¹‚é
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
