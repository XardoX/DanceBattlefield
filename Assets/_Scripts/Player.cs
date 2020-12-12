using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMove moveSettings = default;
    

    private float _time;
    private bool _moved;
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
        moveSettings.nextPoint.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSettings.nextPoint.position, moveSettings.speed * Time.deltaTime);
        _time -= Time.deltaTime;
        if(_time <= 0f && !_moved)
        {
            if(Vector3.Distance(transform.position, moveSettings.nextPoint.position) <= .5f)
            {
                if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if(!Physics2D.OverlapCircle(moveSettings.nextPoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, moveSettings.blocksMovement))
                    {
                        moveSettings.nextPoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * moveSettings.distance, 0f,0f);
                        _time = moveSettings.cooldown;
                        _moved = true;  
                    }
                }else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if(!Physics2D.OverlapCircle(moveSettings.nextPoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, moveSettings.blocksMovement))
                    {
                        moveSettings.nextPoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*moveSettings.distance,0f);
                        _time = moveSettings.cooldown; 
                        _moved = true; 
                    }

                }
            }
        }
    }
    private void LateUpdate() 
    {
        if(Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
        {
            _moved = false;
        }
    }
    [System.Serializable]
    private class PlayerMove
    {
    public float speed = 5f;
    public float cooldown = 0.5f;
    public float distance = 1.5f;
    public Transform nextPoint = null;

    public LayerMask blocksMovement = default;
    }
}
