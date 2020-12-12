using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DanceTile : MonoBehaviour
{
    public float colorCooldown;
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
            _renderer.DOColor(_color, colorCooldown);
            _colorTime = colorCooldown;
        }
    }
}
