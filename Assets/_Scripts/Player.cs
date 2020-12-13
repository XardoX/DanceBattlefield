using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public GameObject shield;
    public int currentDance;
    public int health;
    public float timeToRecovery = 1f;
    [SerializeField]
    private Move moveSettings = default;
    [SerializeField]
    private DanceTypes dance = default;
    [SerializeField]
    private Animator _anim = default;
    public SpriteRenderer _renderer;
    public int nextDance;
    private float _time;
    private float _recoveryTime;
    private bool _moved;
    private float _danceTime;
    private float _moveDistance;
    public bool _defence;
    public bool _attack;
    public bool _jump;
    public bool _earnScore;
    private bool _immortality;
    public static Player instance;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } else Destroy(this);
        _renderer = GetComponent<SpriteRenderer>();
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
        if(_immortality)
        {
            _recoveryTime -= Time.deltaTime;
            if (_recoveryTime <= 0f) _immortality = false;
        }
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
                        SetDance(nextDance);
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
                        SetDance(nextDance);
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
            GameManager.instance.Shake();
            dance.nextDances.Enqueue(Random.Range(0,4));
            nextDance = dance.nextDances.Dequeue();
            _danceTime = dance.danceTime;
            UIManager.instance.SetDanceType(dance.nextDances, nextDance);
        }
    }
    private void SetDance(int danceID)
    {
        currentDance = danceID;
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
        //Debug.Log(Time.time +" Current dance: " + (danceID +1) +" next Dance: " + (dance.nextDances.Peek()+ 1));
    }
    private void DefenceDance(bool isActive)
    {
        _defence = isActive;
        shield.SetActive(isActive);
        if(isActive)
        {
            _anim.SetTrigger("Defence");
        }else 
        {

        }
    }

    private void AttackDance(bool isActive)
    {
        _attack = isActive;
        if(isActive)
        {
            _anim.SetTrigger("Attack");
        }else 
        {

        }
    }
    private void JumpDance(bool isActive)
    {
        _jump = isActive;
        if(isActive)
        {
            _anim.SetTrigger("Jump");
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
            _anim.SetTrigger("Score");
        }else 
        {

        }
    }
    private void LoseHealth()
    {
        if(!_immortality)
        {
            AudioManager.instance.Play("Button");
            _immortality = true;
            _recoveryTime = timeToRecovery;
            health--;
            UIManager.instance.SetPlayerLives(health);
            if(health <= 0)
            {
                GameManager.instance.Defeat();
            }
            _renderer.DOFade(0.5f, 0.2f).SetLoops(10, LoopType.Yoyo);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Spider"))
        {
            if(_attack)
            {
                other.GetComponent<IEnemy>().KillEnemy();
                //addpoints
            }else 
            {
                other.GetComponent<IEnemy>().EnemyCooldown();
                if(!_defence)
                {
                    LoseHealth();
                }else
                {
                    DefenceDance(false);
                }
            }
        } else if(other.CompareTag("Laser"))
        {
            if(other.GetComponent<Laser>().isActive)
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
    private class DanceTypes
    {
        public float danceTime;
        [ReadOnly] public Queue<int> nextDances;
    }
}
[System.Serializable]
    public class Move
    {
    public float speed = 5f;
    public float cooldown = 0.5f;
    public float distance = 1.5f;
    public Transform nextPoint = null;

    public LayerMask tileLayer = default;
    }
