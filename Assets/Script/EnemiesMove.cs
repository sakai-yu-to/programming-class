using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMove : MonoBehaviour
{
    public GameObject[] Enemies;
    public float[] EnemySpeeds;
    public float[] EnemyAngles;
    public Vector3[] vec;
    // Start is called before the first frame update
    void Start()
    {
        vec = new Vector3[Enemies.Length];

        for(int i = 0; i < Enemies.Length; i++)
        {
            float rad = EnemyAngles[i] * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            vec[i] = direction * EnemySpeeds[i] * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].transform.position += vec[i];
        }
    }
}
