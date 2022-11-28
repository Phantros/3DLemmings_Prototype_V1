using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBehaviour : IState
{
    private Bunny _bunny;

    public PickaxeBehaviour(Bunny bunny)
    {
        _bunny = bunny;
    }

    public void Tick()
    { 
        if(_bunny.hittingCrate)
        {
            _bunny.speed = 0;
            
        }
        else
        {
            _bunny.Move();
            _bunny.currentJobModel.transform.position = new Vector3(
                _bunny.transform.position.x, 
                _bunny.transform.position.y + 1, 
                _bunny.transform.position.z);
        }
    }

    public void OnEnter() 
    {
        _bunny.InitPickaxe();
        _bunny.rb.mass = 500;
        _bunny.pickaxeState = true; 
    }

    public void OnExit() { }
}
