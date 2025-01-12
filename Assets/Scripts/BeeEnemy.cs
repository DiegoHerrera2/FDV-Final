using System;
using UnityEngine;


public class BeeEnemy : MonoBehaviour
{
    private bool _dead;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private GameObject _player;
    [SerializeField] private GameObject coinPrefab;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        // Rotate the bee randomly to the right or to the left
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            transform.Rotate(0, 180, 0);
        }
        _player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        _dead = false;
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _animator.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if (_dead) return;
        transform.Translate(Vector2.right * (1.5f * Time.deltaTime));
        // If the player is to far away from the bee, return it to the object pool
        if (_player && Vector2.Distance(transform.position, _player.transform.position) > 15)
        {
            StartCoroutine(EnemySpawner.ReturnBeeWithDelay(gameObject, 0));
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            _dead = true;
            GetComponent<Collider2D>().enabled = false;
            transform.localScale = new Vector3(1, -1, 1);
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);
            _animator.enabled = false;
            SpawnCoin();
            // Wait for 1 second before returning the bee to the object pool
            StartCoroutine(EnemySpawner.ReturnBeeWithDelay(gameObject, 1f));
        }
    }
    
    private void SpawnCoin()
    {
        if (coinPrefab)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}