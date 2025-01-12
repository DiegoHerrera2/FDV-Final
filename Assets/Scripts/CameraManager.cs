using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera1;
    [SerializeField] private CinemachineCamera virtualCamera2;
    
    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float impulseForce = 1.0f;

    [SerializeField] private PlayerMovement player;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        virtualCamera1.Priority = 1;
        virtualCamera2.Priority = 0;
        
        player.OnGunPickedUp += () =>
        {
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 1;
        };
        
        player.Shoot += () =>
        {
            _impulseSource.GenerateImpulse(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized * impulseForce);
        };
    }
    
    
}
