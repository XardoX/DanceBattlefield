using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DanceTile : MonoBehaviour
{
    public Vector2 colorCooldown;
    private SpriteRenderer _renderer;
    private float _colorTime;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>(); 
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
}
