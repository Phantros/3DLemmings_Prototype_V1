using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLeftBehaviour : IState
{
    private Bunny _bunny;

    public GoLeftBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick()
    {
    }

    //Make the bunny face the other bunnies
    //set tag and layer so that the other bunnies collide with this one and do the correct behaviour
    public void OnEnter()
    {
        _bunny.transform.Rotate(0, 180, 0);
        _bunny.InitArrowLeft();
        _bunny.gameObject.tag = "GoLeft";
        _bunny.gameObject.layer = 8;
        _bunny.rb.mass = 500;
    }

    public void OnExit() { }
}
