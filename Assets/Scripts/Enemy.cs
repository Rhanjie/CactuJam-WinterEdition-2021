using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float minSpeed = 10;
    public float maxSpeed = 20;
    
    private float _speed = 12f;
    private Player target;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        /*transform.position = Vector2.MoveTowards(
            transform.position, 
            target.transform.position, 
            _speed * Time.deltaTime
        );*/
        
        var velocity = Time.fixedDeltaTime * _speed * transform.forward;
        transform.position += velocity;
        
        transform.LookAt(target.transform.position);
    }
}
