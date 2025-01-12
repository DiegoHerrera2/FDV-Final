using System;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    
    private State _state = State.Idle;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float thrust = 30.0f;
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private float reloadTime = 0.25f;
    [SerializeField] private Transform bulletOrigin;
    private bool _pickedUpGun = false;
    private float _reloadTimer;
    private int _lastMoveX;
    private float _speedY;
    
    
    private bool _isGrounded;
    private bool _jumpPressed;
    
    public event Action OnGunPickedUp;
    public event Action Shoot;
    public event Action OnDeath;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip deathSound;
    
    
    private enum State
    {
        Idle,
        Walk
    }
    
    private static readonly int Shooting = Animator.StringToHash("Shooting");
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int PickedUpGun = Animator.StringToHash("PickedUpGun");
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        
        switch (_state)
        {
            case State.Idle:
                animator.SetBool(Moving, false);
                if (x != 0)
                {
                    _state = State.Walk;
                }
                break;    
            case State.Walk:
                animator.SetBool(Moving, true);
                spriteRenderer.transform.rotation = _lastMoveX < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                _lastMoveX = x > 0 ? 1 : -1;
                if (x == 0)
                {
                    _state = State.Idle;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPressed = true;
        }
        
        if (Input.GetMouseButtonDown(0) && _reloadTimer <= 0 && _pickedUpGun)
        {
            Shoot?.Invoke();
            audioSource.PlayOneShot(shootSound);
            animator.SetTrigger(Shooting);
            var bulletInstance = Instantiate(bullet, bulletOrigin.position, spriteRenderer.transform.rotation);
            
            _reloadTimer = reloadTime;
        }
        if (_reloadTimer > 0) _reloadTimer -= Time.deltaTime;
        
        if (transform.position.y < -8) Death();
        
    }
    private void FixedUpdate()
    {
        var x = Input.GetAxisRaw("Horizontal");

        var velocity = new Vector2(x * speed * Time.fixedDeltaTime, _speedY);

        _rb.linearVelocity = velocity;
        _speedY += _rb.gravityScale * Physics2D.gravity.y * Time.fixedDeltaTime;
        
        CheckGrounded();
        
        if (_jumpPressed && _isGrounded)
        {
            _speedY = thrust;
            _isGrounded = false;
            audioSource.PlayOneShot(jumpSound);
        }
        
        _jumpPressed = false;
    }
    
    private void CheckGrounded()
    {
        var hitLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.23f, Vector2.down, 0.55f, groundLayer);
        var hitRight = Physics2D.Raycast(transform.position + Vector3.right * 0.23f, Vector2.down, 0.55f, groundLayer);
        
        _isGrounded = hitLeft.collider is not null || hitRight.collider is not null;
        if (_isGrounded) _speedY = _speedY < 0 ? 0 : _speedY;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            Destroy(other.gameObject);
            animator.SetTrigger(PickedUpGun);
            OnGunPickedUp?.Invoke();
            _pickedUpGun = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }
    
    private void Death()
    {
        _rb.linearVelocity = Vector2.zero;
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        _rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        _collider.enabled = false;
        enabled = false;
        animator.enabled = false;
        OnDeath?.Invoke();
        audioSource.PlayOneShot(deathSound);
    }
}