using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LifeCounter : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = "Å~ " + Gamemanager.instance.life.ToString();

    }
}
