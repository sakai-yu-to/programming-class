using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWarp : MonoBehaviour
{
    public int doorIndex;
    public BossManager bossManager;
    // Start is called before the first frame update
    void Start()
    {
        bossManager = FindObjectOfType<BossManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("door hits");

        if (other.CompareTag("Player"))
        {
            bossManager.WarpPlayer(other.transform, doorIndex);
        }
    }
}
