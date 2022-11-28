using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviour : IState
{
    private Bunny _bunny;

    private Vector3 _lastPosition = Vector3.zero;
    public float TimeStuck;

    private readonly int bunnyLayer = 7;

    public WalkBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    //Bunnies don't collide with other bunnies
    //If a bunny gets stuck, it moves position to continue walking
    public void Tick()
    {
        Physics.IgnoreLayerCollision(bunnyLayer, bunnyLayer, true);

        _bunny.Move();

        if (Vector3.Distance(_bunny.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _bunny.transform.position;
    }

    public void OnEnter()
    {
        TimeStuck = 0f;
    }

    public void OnExit() {}
}
