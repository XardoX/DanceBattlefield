using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    public GameObject enemiesParent;
    public GameObject tileParent;
    public List<DanceTile> danceTiles;
    public Camera cam;
    public static GameManager instance;
    public GameObject movePointsParent;
    public List<GameObject> currentEnemies;
    public List<GameObject> enemiesPrefabs;
    public int maxEnemies = 3;

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
        Time.timeScale = 0f;
        foreach(Transform child in tileParent.transform)
        {
            danceTiles.Add(child.GetComponent<DanceTile>());
        }
    }
    void Update()
    {
        if(currentEnemies.Count < maxEnemies)
        {
            int tile = Random.Range(0, danceTiles.Count);
            GameObject newEnemy = Instantiate(enemiesPrefabs[Random.Range(0,enemiesPrefabs.Count)] , danceTiles[tile].transform.position, danceTiles[tile].transform.rotation, enemiesParent.transform);
            newEnemy.GetComponent<IEnemy>().EnemyCooldown();
            currentEnemies.Add(newEnemy);
        }
    }

    public void Defeat()
    {
        UIManager.instance.ShowDefeatWindow();
        AudioManager.instance.Play("Game Over");
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
