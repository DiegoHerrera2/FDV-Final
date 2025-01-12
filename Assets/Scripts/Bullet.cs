using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float lifeTime = 2.0f;
    
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private AudioSource _audioSource;
    
    private void Start()
    {
        Destroy(gameObject, lifeTime);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        
    }
    private void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) 
        {
            CancelInvoke();
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            _audioSource.Play();
            Destroy(gameObject, _audioSource.clip.length);
        }
            
    }
}
