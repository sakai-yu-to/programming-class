using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class risingGround: MonoBehaviour
{
    private Rigidbody2D groundrb = null;
    private Vector3 originalPosition;
    private bool createFlag;
    public GameObject upGround;
    private float movingGroundspeed;
    public float GroundSpeed;
    private Collider2D groundCollider;
    public float highestPosition = 9;
    public float lowestPosition = 0;
    public bool fall = false;
    // Start is called before the first frame update
    void Start()
    {
        groundrb = GetComponent<Rigidbody2D>();
        groundCollider = GetComponent<Collider2D>(); // Collider ‚ðŽæ“¾
        originalPosition = transform.position;
        movingGroundspeed = GroundSpeed;

}

// Update is called once per frame
void Update()
    {
        if(transform.position.y > 0 && !createFlag)
        {
            Instantiate(upGround, originalPosition, Quaternion.identity);
            createFlag = true;
        }

        if(transform.position.y >= originalPosition.y && transform.position.y < 9 )
        {
            groundrb.velocity = new Vector2(0, movingGroundspeed);
        }
        else if(transform.position.y < originalPosition.y && !createFlag)
        {
            Instantiate(upGround, originalPosition, Quaternion.identity);
            createFlag = true;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Foot") && fall)
        {
            movingGroundspeed = -7;
            groundCollider.isTrigger = true; // isTrigger‚ðON‚É‚·‚é
        }
    }
    
}


