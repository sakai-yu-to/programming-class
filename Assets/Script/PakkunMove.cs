using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakkunMove : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D pakkunrb = null;
    private Vector3 originalPosition;
    private float pakkunTime = 0;
    public float triggerDistance; // ÉvÉåÉCÉÑÅ[Ç™ãﬂÇ√Ç≠ãóó£
    public float stopDistance;
    private bool moveBigin = false;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        pakkunrb = GetComponent<Rigidbody2D>();
        pakkunrb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer <= triggerDistance)
        {
            moveBigin = true;
        }

        if(moveBigin)
        {
            pakkunTime += Time.deltaTime;
            if (pakkunTime < 2)
            {
                pakkunrb.velocity = new Vector2(0, 1);

            }
            else if (pakkunTime < 4)
            {
                pakkunrb.velocity = new Vector2(0, -1);
            }
            else if (distanceToPlayer < stopDistance)
            {
                transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
                pakkunrb.velocity = Vector2.zero;
            }
            else
            {
                pakkunTime = 0;
            }

        }


    }
}
