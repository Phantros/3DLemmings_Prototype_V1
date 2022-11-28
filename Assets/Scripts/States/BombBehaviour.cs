using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : IState
{
    private Bunny _bunny;

    public BombBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick() { }

    public void OnEnter()
    {
        _bunny.InitBombCountDown();
        _bunny.SelfDestruct();
    }

    public void OnExit() { }
}
