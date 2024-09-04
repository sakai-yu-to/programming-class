using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    private string kuribouTag = "Kuribou";
    public PlayerMove playermove;

    public AudioSource kurikillSource;
    public AudioClip kurikillClip;
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
        // �Փ˂����I�u�W�F�N�g�̃^�O��"Kuribou"���m�F
        if (other.CompareTag(kuribouTag))
        {
            playermove.BounceOnEnemy();
            kurikillSource.PlayOneShot(kurikillClip);
            Destroy(other.gameObject);
            Debug.Log("Kuribou defeated by CheckGround");
        }
    }
}
