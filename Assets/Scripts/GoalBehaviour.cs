using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalBehaviour : MonoBehaviour
{

    public BunnySpawner bunnySpawner;
    public TextMeshProUGUI bunniesHomeText;
    public float bunniesHome = 0;
    public float acurateBunnyHome;

    //Some logic to calculate howmany bunnies got home and set it to hud text
    public void Update()
    {
        if (bunniesHome > 0)
        {
            acurateBunnyHome = bunniesHome / bunnySpawner.maxBunnies * 100;
        }

        bunniesHomeText.text = ("Bunnies home: " + acurateBunnyHome + "%");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bunny"))
        {
            bunniesHome++;
        }
    }
}
