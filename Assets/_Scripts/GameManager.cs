using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public GameObject movePointsParent;

    public Color[] tilesColors;
    private int _score;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } else Destroy(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Defeat()
    {

    }

    public void AddScore(int value)
    {
        _score += value;
        UIManager.instance.SetScore(_score);
    }
}
