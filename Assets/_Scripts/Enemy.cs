using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMove moveSettings = default;
    [SerializeField] [ReadOnly]private float _moveTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        moveSettings.nextPoint.parent = GameManager.instance.moVePointsParent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillEnemy()
    {
        Debug.Log("Enemy Died");
    }

    [System.Serializable]
    private class EnemyMove
    {
    public float speed = 5f;
    public float cooldown = 0.5f;
    public float distance = 1.5f;
    public Transform nextPoint = null;

    public LayerMask tileLayer = default;
    }
}
