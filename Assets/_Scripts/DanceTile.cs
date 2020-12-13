using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DanceTile : MonoBehaviour
{
    public Sprite tile, highlited;
    public Vector2 colorCooldown;
    public bool occupied;
    private SpriteRenderer _renderer;
    private Collider2D _coll;
    private float _colorTime;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>(); 
        _coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _colorTime -= Time.deltaTime;
        if(_colorTime < 0)
        {
            Color _color = GameManager.instance.tilesColors[Random.Range(0, GameManager.instance.tilesColors.Length)];
            _renderer.DOColor(_color, Random.Range(colorCooldown.x, colorCooldown.y));
            _colorTime = colorCooldown.y;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            _renderer.sprite = highlited;
        }else if(other.CompareTag("Enemy")|| other.CompareTag("Spider"))
        {
            _renderer.sprite = tile;
            gameObject.layer = LayerMask.NameToLayer("Block Tile");
            occupied = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            _renderer.sprite = tile;
        } else if(other.CompareTag("Enemy")|| other.CompareTag("Spider"))
        {
            _renderer.sprite = tile;
            gameObject.layer = LayerMask.NameToLayer("Tile");
            occupied= false;
        }
    }
}
