using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Enemy : MonoBehaviour, IEnemy
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
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSettings.nextPoint.position, moveSettings.speed * Time.deltaTime);
        _moveTime -= Time.deltaTime;
        Vector3 direction = Vector3.zero;
        Vector2 distanceToPlayer = new Vector2(Player.instance.transform.position.x - transform.position.x, Player.instance.transform.position.y - transform.position.y);
        
        if(Mathf.Abs(distanceToPlayer.x) > Mathf.Abs(distanceToPlayer.y))
        {
            if(distanceToPlayer.x > 0f)
            {
                direction = Vector3.right;
            } else if (distanceToPlayer.x < 0f)
            {
                direction = Vector3.left;
            }
        }else if(Mathf.Abs(distanceToPlayer.x) < Mathf.Abs(distanceToPlayer.y))
        {
            if(distanceToPlayer.y > 0f)
            {
                direction = Vector3.up;
            } else if(distanceToPlayer.y < 0f) 
            {
                direction = Vector3.down;
            }
        } else if (Mathf.Abs(distanceToPlayer.x) == Mathf.Abs(distanceToPlayer.y) && Mathf.Abs(distanceToPlayer.x) !=0f)
        {
            if(distanceToPlayer.x > 0f)
            {
                direction = Vector3.right;
            } else if (distanceToPlayer.x < 0f)
            {
                direction = Vector3.left;
            }
        }else
        {
            direction = Vector3.zero;
        }
        //Debug.Log(distanceToPlayer + " direction: "+ direction);
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
    public void KillEnemy()
    {
        GameManager.instance.AddScore(150);
        Debug.Log("Enemy Died");
        GameManager.instance.currentEnemies.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void EnemyCooldown()
    {
        _moveTime += moveSettings.cooldown * 2;
    }
}