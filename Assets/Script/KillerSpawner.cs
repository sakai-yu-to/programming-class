using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject leftKillerPrefab;
    public GameObject rightKillerPrefab;
    public GameObject goldenLeftKillerPrefab;
    public GameObject goldenRightKillerPrefab;
    public float spawnInterval;
    public Transform[] leftKillerPoints;
    public Transform[] rightKillerPoints;
    public Transform[] goldenLeftKillerPoints;
    public Transform[] goldenRightKillerPoints;

    private bool[] moveBiginL;
    private bool[] moveBiginR;
    private bool[] moveBiginGL;
    private bool[] moveBiginGR;

    public float triggerDistance;
    private bool moveBigin = false;
    /*
    private float[] timerL;
    private float[] timerR;
    private float[] timerGL;
    private float[] timerGR;
    */
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distanceToPlayer <= triggerDistance)
        {
            Debug.Log("fall goomba");
            moveBigin = true;
        }

        if (moveBigin)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                for (int i = 0; i < leftKillerPoints.Length; i++)
                {
                    Instantiate(leftKillerPrefab, leftKillerPoints[i].position, Quaternion.identity);
                }

                for (int i = 0; i < rightKillerPoints.Length; i++)
                {
                    Instantiate(rightKillerPrefab, rightKillerPoints[i].position, Quaternion.identity);
                }

                for (int i = 0; i < goldenLeftKillerPoints.Length; i++)
                {
                    Instantiate(goldenLeftKillerPrefab, goldenLeftKillerPoints[i].position, Quaternion.identity);
                }

                for (int i = 0; i < goldenRightKillerPoints.Length; i++)
                {
                    Instantiate(goldenRightKillerPrefab, goldenRightKillerPoints[i].position, Quaternion.identity);
                }

                timer = 0.0f; // タイマーをリセット
            }

        }

    }
}
