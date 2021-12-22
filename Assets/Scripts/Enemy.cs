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

    public AudioClip[] sounds;

    public Animator animator;

    private float power = 10;
    
    private float _speed = 12f;
    private Player target;

    private bool attacked = false;
    private float attackTime = 1.2f;
    private float timer = 0;

    private bool disabledScript = false;
    
    private Rigidbody rigidbodyy;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _speed = Random.Range(minSpeed, maxSpeed);
        
        rigidbodyy = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (disabledScript)
        {
            return;
        }
        
        if (sounds.Length > 0 && _audioSource.isPlaying == false && Random.Range(0, 100) <= 2)
        {
            _audioSource.clip = sounds[Random.Range(0, sounds.Length - 1)];
            _audioSource.Play();
        }
        
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
        if (disabledScript)
        {
            return;
        }
        
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
        disabledScript = true;
        this.enabled = false;

        yield return new WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (disabledScript)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Player") && !attacked) {
            target.DealDamage(power);

            DealDamage(0);

            attacked = true;

            return;
        }
    }
    
    private void OnTriggerStay(Collider other) {
        if (disabledScript)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Player") && !attacked) {
            target.DealDamage(power);

            DealDamage(0);

            attacked = true;

            return;
        }
    }
}
