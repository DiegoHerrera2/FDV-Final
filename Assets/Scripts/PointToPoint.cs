using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPoint : MonoBehaviour
{
    private Vector3 _tempPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;

    private void Start()
    {
        _tempPosition = transform.position;
    }
    private void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        transform.Translate(direction.normalized * (speed * Time.deltaTime));
        
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            (target.position, _tempPosition) = (_tempPosition, target.position);
        }
        
    }
}