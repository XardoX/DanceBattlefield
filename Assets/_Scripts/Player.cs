using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    public int[] testList;
    public int health;
    [SerializeField]
    private PlayerMove moveSettings = default;
    [SerializeField]
    private DanceTypes dance = default;
    private float _time;
    private bool _moved;
    private float _danceTime;
    private float _moveDistance;
    private bool _defence;
    private bool _attack;
    private bool _jump;
    private bool _earnScore;
    public static Player instance;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } else Destroy(this);
    }

    void Start()
    {
        moveSettings.nextPoint.parent = GameManager.instance.movePointsParent.transform;
        dance.nextDances = new Queue<int>();
        for(int i = 0; i < 4; i++)
        {
            dance.nextDances.Enqueue(Random.Range(0,4));
        }
        UIManager.instance.SetDanceType(dance.nextDances);
        _moveDistance = moveSettings.distance;
    }

    void Update()
    {
        Move();
        Dance();
    }
    private void LateUpdate() 
    {
        if(Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
        {
            _moved = false;
        }
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSettings.nextPoint.position, moveSettings.speed * Time.deltaTime);
        _time -= Time.deltaTime;
        if(_time <= 0f && !_moved)
        {
            if(Vector3.Distance(transform.position, moveSettings.nextPoint.position) <= .5f)
            {
                if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if(Physics2D.OverlapCircle(moveSettings.nextPoint.position + new Vector3(Input.GetAxisRaw("Horizontal")*_moveDistance, 0f, 0f), .2f, moveSettings.tileLayer))
                    {
                        moveSettings.nextPoint.position += new Vector3(Input.GetAxisRaw("Horizontal") *_moveDistance, 0f,0f);
                        _time = moveSettings.cooldown;
                        _moved = true;  
                        if(_earnScore)
                        {
                            GameManager.instance.AddScore(100);
                        }
                    }
                }else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if(Physics2D.OverlapCircle(moveSettings.nextPoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical")*_moveDistance, 0f), .2f, moveSettings.tileLayer))
                    {
                        moveSettings.nextPoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*_moveDistance,0f);
                        _time = moveSettings.cooldown; 
                        _moved = true; 
                        if(_earnScore)
                        {
                            GameManager.instance.AddScore(100);
                        }
                    }

                }
            }
        }
    }

    private void Dance()
    {
        _danceTime -= Time.deltaTime;
        if(_danceTime <= 0f)
        {
            dance.nextDances.Enqueue(Random.Range(0,4));
            SetDance(dance.nextDances.Dequeue());
            _danceTime = dance.danceTime;
        }
    }
    private void SetDance(int danceID)
    {
        DefenceDance(false);
        AttackDance(false);
        JumpDance(false);
        ScoreDance(false);
        switch(danceID)
        {
            case 0:
                DefenceDance(true);
                break;
            case 1:
                AttackDance(true);
                break;
            case 2:
                JumpDance(true);
                break;
            case 3:
                ScoreDance(true);
                break;
            default:
                break;
        }
        UIManager.instance.SetDanceType(dance.nextDances);
        //Debug.Log(Time.time +" Current dance: " + (danceID +1) +" next Dance: " + (dance.nextDances.Peek()+ 1));
    }
    private void DefenceDance(bool isActive)
    {
        _defence = isActive;
        if(isActive)
        {
            //animacja albo efekt
        }else 
        {

        }
    }

    private void AttackDance(bool isActive)
    {
        _attack = isActive;
        if(isActive)
        {
            //animacja albo efekt
        }else 
        {

        }
    }
    private void JumpDance(bool isActive)
    {
        _jump = isActive;
        if(isActive)
        {
            _moveDistance = moveSettings.distance * 2;
        }else 
        {
            _moveDistance = moveSettings.distance;
        }
    }

    private void ScoreDance(bool isActive)
    {
        _earnScore = isActive;
        if(isActive)
        {

        }else 
        {

        }
        
    }
    private void LoseHealth()
    {
        health--;
        if(health <= 0)
        {
            GameManager.instance.Defeat();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy"))
        {
            if(_attack)
            {
                other.GetComponent<Enemy>().KillEnemy();
                //addpoints
            }else 
            {
                if(!_defence)
                {
                    LoseHealth();
                }else
                {
                    DefenceDance(false);
                }
            }
            
        }
    }
    [System.Serializable]
    private class PlayerMove
    {
    public float speed = 5f;
    public float cooldown = 0.5f;
    public float distance = 1.5f;
    public Transform nextPoint = null;

    public LayerMask tileLayer = default;
    }

    [System.Serializable]
    private class DanceTypes
    {
        public float danceTime;
        [ReadOnly] public Queue<int> nextDances;
    }
}
