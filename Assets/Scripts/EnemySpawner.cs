using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject wormEnemyPrefab;
    [SerializeField] private GameObject beeEnemyPrefab;
    [SerializeField] private float spawnRate = 1.0f;
    [SerializeField] private List<Transform> spawnPoints;
    private float _timer;

    private static IObjectPool<GameObject> _wormPool;
    private static IObjectPool<GameObject> _beePool;
    
    private static EnemySpawner _instance;
    
    public static EnemySpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemySpawner>();
            }

            return _instance;
        }
    }
    
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnRate)
        {
            _timer = 0;
            SpawnEnemy();
        }
    }
    
    private void SpawnEnemy()
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        var enemy = Random.Range(0, 2) == 0 ? _wormPool.Get() : _beePool.Get();
        enemy.transform.position = spawnPoint.position;
    }
    
    
    // Make it using Object Pooling
    
    private void Start()
    {
        _wormPool = new ObjectPool<GameObject>(() => Instantiate(wormEnemyPrefab),
            (obj) => obj.SetActive(true), 
            (obj) => obj.SetActive(false), 
            (obj) => Destroy(obj), false, 100);
        
        _beePool = new ObjectPool<GameObject>(() => Instantiate(beeEnemyPrefab),
            (obj) => obj.SetActive(true),
            obj => obj.SetActive(false),
            obj => Destroy(obj), false, 100);
    }
    
    public static IEnumerator ReturnBeeWithDelay(GameObject bee, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _beePool.Release(bee);
    }
    
    public static IEnumerator ReturnWormWithDelay(GameObject worm, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _wormPool.Release(worm);
    }
    
}
