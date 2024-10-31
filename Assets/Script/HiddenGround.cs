using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenGround : MonoBehaviour
{
    public PlayerMove playerMove;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    public changeValue changevalue;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gamemanager = GameObject.Find("Gamemanager");
        if (gamemanager != null)
        {
            changevalue = gamemanager.GetComponent<changeValue>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        Color currentColor = spriteRenderer.color;
        float initialAlpha = 0.0f;  
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, initialAlpha);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Head") && playerMove.ySpeed > 0)
        {
            playerMove.ySpeed = -changevalue.playerGravity;
            collider2D.isTrigger = false;
            Color currentColor = spriteRenderer.color;
            float newAlpha = 1.0f;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            gameObject.tag = "Ground";
        }
    }
}
