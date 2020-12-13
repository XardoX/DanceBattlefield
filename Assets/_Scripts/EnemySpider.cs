using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;


public class EnemySpider : MonoBehaviour, IEnemy
{
    [SerializeField] private LayerMask playerLayer = default;
    [SerializeField] private SpriteRenderer laser = default;
    [SerializeField] private Move moveSettings = default;
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] [ReadOnly]private float _moveTime = 0f;
    [SerializeField] [ReadOnly]private float rotationTime = 0f;
    private bool _moved;
    private bool _shooting;
    private int rotationCount = 4;
    void Start()
    {
        moveSettings.nextPoint.parent = GameManager.instance.movePointsParent.transform;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.up * moveSettings.distance, Color.green, .5f);
        if(_shooting)
        {

        }else
        {
            if(_moved)
            {
                rotationTime -= Time.deltaTime;
                if(rotationTime <= 0f)
                {
                    if(rotationCount > 0)
                    {
                        Rotate();
                    } else _moved = false;
                }
            } else 
            {
                Move();
            }
        }
    }
    private void Rotate()
    {
        rotationCount--;
        rotationTime = rotationDuration * 1.5f;
        _moved = true;
        transform.DORotate(Vector3.forward * 90f, rotationDuration, RotateMode.LocalAxisAdd).OnComplete(Scan);
    }
    private void Scan()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveSettings.distance *3);
        //if(hit.collider != null) Debug.Log(hit.transform.name);
        if(Physics2D.Raycast(transform.position, -transform.up, moveSettings.distance *3, playerLayer))
        {
            _shooting = true;
            _moved = true;
            _moveTime = moveSettings.cooldown;
            rotationCount = 4;
            Prepare();
        }
    }

    private void Prepare()
    {
        laser.gameObject.SetActive(true);
        laser.DOFade(0.5f, 0.5f).OnComplete(Shoot);
    }
    private void Shoot()
    {
        laser.DOFade(1f, 0.1f);
        laser.GetComponent<Laser>().isActive = true;
        _shooting = false;
        rotationTime = rotationDuration * 3;
        rotationCount = 4;
        _moved = false;
        _moveTime = moveSettings.cooldown * 6;
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
        if(_moveTime <= 0f)
        {
            if(Vector3.Distance(transform.position, moveSettings.nextPoint.position) <= .5f)
            {  
                if(Physics2D.OverlapCircle(moveSettings.nextPoint.position + direction * moveSettings.distance, .2f, moveSettings.tileLayer))
                {
                    moveSettings.nextPoint.position += direction * moveSettings.distance;
                    _moveTime = moveSettings.cooldown;
                    _moved = true;
                    rotationCount = 4;
                    rotationTime = rotationDuration;
                }
            }
        }
    }

    public void KillEnemy()
    {
        Debug.Log("Spider Died");
        Destroy(this.gameObject);
    }

    public void EnemyCooldown()
    {
        _moveTime += moveSettings.cooldown * 2;
    }
}
