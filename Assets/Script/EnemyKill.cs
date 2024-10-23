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

    public int killHammer = 0;

    public bool touchLamp = false;
    // Start is called before the first frame update
    void Start()
    {
        killHammer = 0;
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
            killKuribou();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("HammerBro"))
        {
            killKuribou();
            Destroy(other.gameObject);
            killHammer++;
        }


        if (other.CompareTag("Killer") || other.CompareTag("FlyingKuribou"))
        {
            playermove.killKiller = true;   
            playermove.BounceOnEnemy();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("breakGround"))
        {
            breakSource.Play();
        }

        if (other.CompareTag("BossLamp"))
        {
            killKuribou();
            Debug.Log("hit bossLamp");
            touchLamp = true;
        }
    }

    void killKuribou()
    {
        playermove.killKuribou = true;
        playermove.BounceOnEnemy();
        kurikillSource.PlayOneShot(kurikillClip);
        Debug.Log("Kuribou defeated by CheckGround");

    }
}
