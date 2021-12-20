using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float minSpeed = 10;
    public float maxSpeed = 20;
    
    public float HP = 10.0f;
    public float maxHP = 10.0f;

    public Animator animator;
    
    private float power = 10;
    
    private float _speed = 12f;
    private Player target;

    private bool attacked = false;
    private float attackTime = 0.2f;
    private float timer = 0;
    
    private Rigidbody rigidbodyy;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _speed = Random.Range(minSpeed, maxSpeed);
        
        rigidbodyy = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (attacked)
        {
            timer += Time.deltaTime;

            if (timer >= attackTime)
            {
                attacked = false;
                timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        var velocity = Time.fixedDeltaTime * _speed * transform.forward;
        transform.position += velocity;
        
        transform.LookAt(target.transform.position);
    }

    public void DealDamage(float damage)
    {
        if (damage > 0)
        {
            animator.SetTrigger("TakeDamage");
            HP -= damage;
        }

        if (HP <= 0)
        {
            Die();

            return;
        }
        
        rigidbodyy.AddForce(Vector3.forward * -400);
        rigidbodyy.AddForce(Vector3.up * 300);
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !attacked) {
            target.DealDamage(power);

            DealDamage(0);

            attacked = true;

            return;
        }
    }
}
