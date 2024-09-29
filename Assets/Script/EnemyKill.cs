using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    public PlayerMove playermove;

    public AudioSource kurikillSource;
    public AudioClip kurikillClip;

    public AudioSource breakSource;
    public AudioClip breakClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the tag of the collided object is Kuribou
        if (other.CompareTag("Kuribou"))
        {
            playermove.BounceOnEnemy();
            kurikillSource.PlayOneShot(kurikillClip);
            Destroy(other.gameObject);
            Debug.Log("Kuribou defeated by CheckGround");
        }

        if (other.CompareTag("breakGround"))
        {
            breakSource.Play();
        }
    }
}
