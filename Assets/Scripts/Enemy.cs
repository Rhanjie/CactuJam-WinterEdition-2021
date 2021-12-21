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

    private bool disabledScript = false;
    
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
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    public void DealDamage(float damage)
    {
        if (damage > 0)
        {
            HP -= damage;
        }

        if (HP <= 0)
        {
            StartCoroutine(Die());

            return;
        }

        if (damage > 0)
        {
            animator.SetTrigger("TakeDamage");
        }

        rigidbodyy.AddForce(transform.forward * -400);
        rigidbodyy.AddForce(Vector3.up * 300);
    }
    
    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;

        yield return new WaitForSeconds(1.5f);
        
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
