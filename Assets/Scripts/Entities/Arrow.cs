using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void Init(Vector3 startPosition)
    {
        this.transform.position = startPosition;    
    }
}
