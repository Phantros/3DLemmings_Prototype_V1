using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BunnySpawner : MonoBehaviour
{
    public GameObject bunnyContainer;
    public GameObject startBlock;

    public Bunny aBunnyObjBase;

    public TextMeshProUGUI bunniesInPlayText;

    public List<Bunny> bunnyList = new List<Bunny>();

    public int maxBunnies = 50;
    public int maxCountdown;

    public int amountOfBunnies;

    private int countdown;

    public bool doneSpawning = false;

    private void Awake()
    {
        countdown = 0;
    }

    //Spawn bunnies until maxbunnies is reached
    private void Update()
    {
        bunniesInPlayText.text = ("Bunnies in play: " + amountOfBunnies + "/" + maxBunnies);

        if (!doneSpawning)
        {
            countdown++;

            if (countdown >= maxCountdown)
            {
                InstantiateBunny();
            }
        }
    }

    //If done spawning, update will stop counting down 
    //If not, instantiate a bunny,
    //give the starting position,
    //add it to the bunnyList,
    //put it in the bunnycontainer,
    //and give it a unique name. 
    private void InstantiateBunny()
    {
        if (amountOfBunnies >= maxBunnies)
        {
            doneSpawning = true;
            return;
        }

        else
        {
            Bunny aBunny = Instantiate(aBunnyObjBase);
            aBunny.Init(startBlock.transform.position);

            bunnyList.Add(aBunny);

            aBunny.transform.parent = bunnyContainer.transform;
            aBunny.name = "" + (amountOfBunnies);
            amountOfBunnies++;

            countdown = 0;
        }
    }
}
