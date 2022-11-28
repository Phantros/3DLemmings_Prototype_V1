using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BouncePad : MonoBehaviour
{
    public void Init(Vector3 startPosition)
    {
        this.transform.position = startPosition;
    }
}
