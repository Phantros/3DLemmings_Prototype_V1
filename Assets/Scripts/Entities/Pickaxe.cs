using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{ 
    public void Init(Vector3 startPosition)
    {
        this.transform.position = startPosition;
    }
}