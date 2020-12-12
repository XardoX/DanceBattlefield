using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isActive = false;

    private void OnDisable() 
    {
        isActive = false;
    }
}
