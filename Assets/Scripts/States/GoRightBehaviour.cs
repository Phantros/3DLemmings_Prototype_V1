using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRightBehaviour : IState
{
    private Bunny _bunny;

    public GoRightBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _bunny.transform.Rotate(0, 180, 0);
        _bunny.InitArrowRight();
        _bunny.gameObject.tag = "GoRight";
        _bunny.gameObject.layer = 8;
        _bunny.rb.mass = 500;
    }

    public void OnExit() { }
}
