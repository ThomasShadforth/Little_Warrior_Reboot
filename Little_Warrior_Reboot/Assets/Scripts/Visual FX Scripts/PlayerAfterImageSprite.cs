using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] float _activeTime = 1.85f;
    float _timeActivated;
    float _alpha;
    [SerializeField] float _setAlpha;
    [SerializeField] float _alphaMultiplier;

    Transform _player;

    SpriteRenderer _renderer;
    SpriteRenderer _playerRenderer;

    Color _color;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerRenderer = _player.GetComponent<SpriteRenderer>();

        _alpha = _setAlpha;
        _renderer.sprite = _playerRenderer.sprite;

        transform.position = _player.position;
        transform.localScale = _player.localScale;
        transform.rotation = _player.rotation;

        _timeActivated = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        _alpha *= _alphaMultiplier;
        //_color = new Color(143 / 255, 62 / 255, 255 / 255, _alpha);
        _color = new Color(243 / 255, 28 / 255, 28 / 255, _alpha);
        _renderer.color = _color;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            AfterImageObjectPool.instance.AddToPool(gameObject);
        }
    }
}
