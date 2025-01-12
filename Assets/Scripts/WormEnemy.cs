using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class WormEnemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _timer;
    [SerializeField] private float movementTime = 0.5f;
    private Animator _animator;
    private bool _dead;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private GameObject coinPrefab;
    
    
    private static readonly int Displacement = Animator.StringToHash("Displace");
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _dead = false;
        _animator.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if (_dead) return;
        _timer += Time.deltaTime;
        if (_timer >= movementTime)
        {
            // Cast a ray to check if there is ground in front of the enemy
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.down, 1f, groundLayer);
            if (!hit) transform.Rotate(0, 180, 0);
            
            _timer = 0;
            _animator.SetBool(Displacement, !_animator.GetBool(Displacement));
            _rigidbody2D.AddForce(transform.right * (1.5f * (!_animator.GetBool(Displacement) ? 1 : 0)), ForceMode2D.Impulse);
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
            _rigidbody2D.AddForce(Vector2.up, ForceMode2D.Impulse);
            _animator.enabled = false;
            SpawnCoin();
            // Wait for 1 second before returning the bee to the object pool
            StartCoroutine(EnemySpawner.ReturnWormWithDelay(gameObject, 1f));
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
