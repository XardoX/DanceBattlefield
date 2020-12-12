using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;


public class EnemySpider : MonoBehaviour
{
    [SerializeField] private Move moveSettings = default;
    [SerializeField] [ReadOnly]private float _moveTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        moveSettings.nextPoint.parent = GameManager.instance.movePointsParent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;
        if(_moveTime <= 0f)
        {
            if(Vector3.Distance(transform.position, moveSettings.nextPoint.position) <= .5f)
            {  
                if(Physics2D.OverlapCircle(moveSettings.nextPoint.position + direction * moveSettings.distance, .2f, moveSettings.tileLayer))
                {
                    moveSettings.nextPoint.position += direction * moveSettings.distance;
                    _moveTime = moveSettings.cooldown;
                }
                
            }
        }
    }
}
