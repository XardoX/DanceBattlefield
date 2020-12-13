using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isActive = false;
    public float duration = 1f;
    private float _time;

    private void OnEnable() 
    {
        _time = duration;
    }
    private void Update() 
    {
        _time -= Time.deltaTime;
        if(_time <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable() 
    {
        isActive = false;
    }
}
