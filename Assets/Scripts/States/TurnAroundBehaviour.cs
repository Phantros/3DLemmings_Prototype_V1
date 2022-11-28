using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundBehaviour : IState
{
    private Bunny _bunny;

    public TurnAroundBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick()
    {
    }

    //Spawn arrow above the bunny
    //Make the bunny a 'wall' for the reaction of the other bunnies
    //Make the bunnies layer 8, so the other bunnies collide with this one
    //Set the mass to heavy, to stop unwanted movement
    public void OnEnter()
    {
        _bunny.InitArrowTurnAround();
        _bunny.gameObject.tag = "Wall";
        _bunny.gameObject.layer = 8;
        _bunny.rb.mass = 500;
    }

    public void OnExit()
    {
    }
}
