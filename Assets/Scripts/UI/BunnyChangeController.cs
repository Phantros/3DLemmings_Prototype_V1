using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BunnyChangeController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public Bunny aBunny;
    private GameObject currentBunny;
    public GameObject bunnyContainer;
    public BunnySpawner bunnySpawner;
    public ButtonHandler buttonHandler;

    //The logic for when a player clicks on a bunny
    //If the player first clicked on a possible job in the UI, then the bunny will get that job
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Bunny") || (hit.transform.gameObject.layer == LayerMask.NameToLayer("JobBunny")))
                    {
                        currentBunny = hit.collider.gameObject;

                        aBunny = currentBunny.GetComponent<Bunny>();

                        ChangeJob();

                        buttonHandler.possibleJob = null;
                    }

                }
            }
        }
    }

    //Switch statement to set the possible job
    //Nuke and speedup get handled here, to reach all the bunnies in the scene at once
    private void ChangeJob()
    {
        switch (buttonHandler.possibleJob)
        {
            case "Pickaxe":
                aBunny.newJob = "Pickaxe";
                break;

            case "Bounce":
                aBunny.newJob = "Bounce";
                break;

            case "Bomb":
                aBunny.newJob = "Bomb";
                break;

            case "GoLeft":
                aBunny.newJob = "GoLeft";
                break;

            case "GoRight":
                aBunny.newJob = "GoRight";
                break;

            case "TurnAround":
                aBunny.newJob = "TurnAround";
                break;

            case "SpeedUp":

                foreach (Bunny bunny in bunnySpawner.bunnyList)
                {
                    bunny.SpeedUp();
                }

                break;

            case "Nuke":
                //ToDo add timer, end level logic
                bunnySpawner.doneSpawning = true;
                Destroy(bunnyContainer);
                break;

            default:
                break;
        }
    }
}
