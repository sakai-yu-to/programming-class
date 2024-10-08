using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menukey : MonoBehaviour
{
    public GameObject[] pressKey;
    public GameObject hardImage;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < pressKey.Length; i++)
        {
            pressKey[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Gamemanager.instance.isHard)
        {
            hardImage.SetActive(true);
        }
        else
        {
            hardImage.SetActive(false);
        }
        


        if (Input.GetKey(KeyCode.W))
        {
            pressKey[0].SetActive(true);
        }
        else
        {
            pressKey[0].SetActive(false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            pressKey[1].SetActive(true);
        }
        else
        {
            pressKey[1].SetActive(false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            pressKey[2].SetActive(true);
        }
        else
        {
            pressKey[2].SetActive(false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            pressKey[3].SetActive(true);
        }
        else
        {
            pressKey[3].SetActive(false);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            pressKey[4].SetActive(true);
        }
        else
        {
            pressKey[4].SetActive(false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pressKey[5].SetActive(true);
        }
        else
        {
            pressKey[5].SetActive(false);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            pressKey[6].SetActive(true);
        }
        else
        {
            pressKey[6].SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            pressKey[7].SetActive(true);
        }
        else
        {
            pressKey[7].SetActive(false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            pressKey[8].SetActive(true);
        }
        else
        {
            pressKey[8].SetActive(false);
        }
        
    }
}
