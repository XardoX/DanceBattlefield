using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    
    public Camera cam;
    public static GameManager instance;
    public GameObject movePointsParent;

    public Color[] tilesColors;
    private int _score;

    [SerializeField] private float shakeDuration = 0.1f,shakeMagnitude = 0.1f;
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
    [Button]
    public void Shake()
    {
        StartCoroutine(ShakeCamera(shakeDuration,shakeMagnitude));
    }
    public void Shake (float duration, float magnitude)
    {
        StartCoroutine(ShakeCamera(duration,magnitude));
    }

    public IEnumerator ShakeCamera (float duration, float magnitude)
    {
        Vector3 originalPos = cam.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.localPosition = new Vector3(x, y, originalPos.z);
        
            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.transform.localPosition = originalPos;

    }
}
