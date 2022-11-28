using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BounceBehaviour : IState
{
    private Bunny _bunny;

    public BounceBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _bunny.InitBouncePad();
        _bunny.gameObject.tag = "Bounce";
        _bunny.gameObject.layer = 8;
        _bunny.rb.mass = 500;
    }

    public void OnExit() 
    {

    }
}
