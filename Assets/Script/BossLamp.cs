using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLamp : MonoBehaviour
{
    public float rotationSpeed = 360f; // ‰ñ“]‘¬“x (1•bŠÔ‚É360“x‰ñ“])
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
