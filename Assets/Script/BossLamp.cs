using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLamp : MonoBehaviour
{
    public float rotationSpeed = 360f; // ��]���x (1�b�Ԃ�360�x��])
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }


}
