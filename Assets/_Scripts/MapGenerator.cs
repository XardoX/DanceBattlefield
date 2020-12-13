using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 mapSize;
    public float tileDistance;
    void Start()
    {
        GenerateMap();
    }
    private void GenerateMap()
    {
        Vector3 nextSpawn = new Vector3(mapSize.x * -tileDistance/2 + tileDistance/2, mapSize.y * -tileDistance/2 + tileDistance/2, 0f);
        for(int width = 0; width < mapSize.x; width++)
        {
            for(int height = 0; height < mapSize.y; height++)
            {
                Instantiate(tilePrefab, nextSpawn, transform.rotation, transform);
                nextSpawn += new Vector3(0f, tileDistance, 0f);
            }
            nextSpawn += new Vector3(tileDistance, -tileDistance * mapSize.y, 0f);
        }
    }
}
