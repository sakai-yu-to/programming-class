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

    public float triggerDistance;

    // Separate moveBigin arrays for each type of killer
    public bool[] moveBiginL;
    public bool[] moveBiginR;
    public bool[] moveBiginGL;
    public bool[] moveBiginGR;

    // Separate timers for each spawn point array
    public float[] timerL;
    public float[] timerR;
    public float[] timerGL;
    public float[] timerGR;

    void Start()
    {
        // Initialize the moveBigin and timer arrays
        moveBiginL = new bool[leftKillerPoints.Length];
        moveBiginR = new bool[rightKillerPoints.Length];
        moveBiginGL = new bool[goldenLeftKillerPoints.Length];
        moveBiginGR = new bool[goldenRightKillerPoints.Length];

        timerL = new float[leftKillerPoints.Length];
        timerR = new float[rightKillerPoints.Length];
        timerGL = new float[goldenLeftKillerPoints.Length];
        timerGR = new float[goldenRightKillerPoints.Length];

        // Set initial values for moveBigin and timer arrays
        for (int i = 0; i < leftKillerPoints.Length; i++)
        {
            moveBiginL[i] = false;
            timerL[i] = 0;
        }
        for (int i = 0; i < rightKillerPoints.Length; i++)
        {
            moveBiginR[i] = false;
            timerR[i] = 0;
        }
        for (int i = 0; i < goldenLeftKillerPoints.Length; i++)
        {
            moveBiginGL[i] = false;
            timerGL[i] = 0;
        }
        for (int i = 0; i < goldenRightKillerPoints.Length; i++)
        {
            moveBiginGR[i] = false;
            timerGR[i] = 0;
        }
    }

    void Update()
    {
        // Check and spawn left killers
        for (int i = 0; i < leftKillerPoints.Length; i++)
        {
            float distanceToPlayer = player.transform.position.x - leftKillerPoints[i].position.x;
            bool withinTriggerDistance = distanceToPlayer <= triggerDistance && distanceToPlayer >= -triggerDistance;

            // If the player just entered the trigger distance, spawn immediately
            if (withinTriggerDistance && !moveBiginL[i])
            {
                moveBiginL[i] = true;
                Instantiate(leftKillerPrefab, leftKillerPoints[i].position, Quaternion.identity);
                timerL[i] = 0; // Reset the timer for subsequent spawns
            }

            // Continue spawning at regular intervals
            if (moveBiginL[i])
            {
                timerL[i] += Time.deltaTime;
                if (timerL[i] >= spawnInterval)
                {
                    Instantiate(leftKillerPrefab, leftKillerPoints[i].position, Quaternion.identity);
                    timerL[i] = 0;
                }
            }

            // If the player moves out of trigger distance, stop spawning
            if (!withinTriggerDistance)
            {
                moveBiginL[i] = false;
            }
        }

        // Check and spawn right killers
        for (int i = 0; i < rightKillerPoints.Length; i++)
        {
            float distanceToPlayer = player.transform.position.x - rightKillerPoints[i].position.x;
            bool withinTriggerDistance = distanceToPlayer <= triggerDistance && distanceToPlayer >= -triggerDistance;

            if (withinTriggerDistance && !moveBiginR[i])
            {
                moveBiginR[i] = true;
                Instantiate(rightKillerPrefab, rightKillerPoints[i].position, Quaternion.identity);
                timerR[i] = 0;
            }

            if (moveBiginR[i])
            {
                timerR[i] += Time.deltaTime;
                if (timerR[i] >= spawnInterval)
                {
                    Instantiate(rightKillerPrefab, rightKillerPoints[i].position, Quaternion.identity);
                    timerR[i] = 0;
                }
            }

            if (!withinTriggerDistance)
            {
                moveBiginR[i] = false;
            }
        }

        if (Gamemanager.instance.isHard)
        {
            // Check and spawn golden left killers
            for (int i = 0; i < goldenLeftKillerPoints.Length; i++)
            {
                float distanceToPlayer = player.transform.position.x - goldenLeftKillerPoints[i].position.x;
                bool withinTriggerDistance = distanceToPlayer <= triggerDistance && distanceToPlayer >= -triggerDistance;

                if (withinTriggerDistance && !moveBiginGL[i])
                {
                    moveBiginGL[i] = true;
                    Instantiate(goldenLeftKillerPrefab, goldenLeftKillerPoints[i].position, Quaternion.identity);
                    timerGL[i] = 0;
                }

                if (moveBiginGL[i])
                {
                    timerGL[i] += Time.deltaTime;
                    if (timerGL[i] >= spawnInterval)
                    {
                        Instantiate(goldenLeftKillerPrefab, goldenLeftKillerPoints[i].position, Quaternion.identity);
                        timerGL[i] = 0;
                    }
                }

                if (!withinTriggerDistance)
                {
                    moveBiginGL[i] = false;
                }
            }

            // Check and spawn golden right killers
            for (int i = 0; i < goldenRightKillerPoints.Length; i++)
            {
                float distanceToPlayer = player.transform.position.x - goldenRightKillerPoints[i].position.x;
                bool withinTriggerDistance = distanceToPlayer <= triggerDistance && distanceToPlayer >= -triggerDistance;

                if (withinTriggerDistance && !moveBiginGR[i])
                {
                    moveBiginGR[i] = true;
                    Instantiate(goldenRightKillerPrefab, goldenRightKillerPoints[i].position, Quaternion.identity);
                    timerGR[i] = 0;
                }

                if (moveBiginGR[i])
                {
                    timerGR[i] += Time.deltaTime;
                    if (timerGR[i] >= spawnInterval)
                    {
                        Instantiate(goldenRightKillerPrefab, goldenRightKillerPoints[i].position, Quaternion.identity);
                        timerGR[i] = 0;
                    }
                }

                if (!withinTriggerDistance)
                {
                    moveBiginGR[i] = false;
                }
            }
        }
    }
}
