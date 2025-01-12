using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    
    public static event Action OnCoinCollected;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        // Add force in a random upward direction, semicircle
        _rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), 2f) * 2f, ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        OnCoinCollected?.Invoke();
        _audioSource.Play();
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        Destroy(gameObject, _audioSource.clip.length);
    }
}