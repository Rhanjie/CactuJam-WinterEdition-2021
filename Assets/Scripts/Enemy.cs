using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IEnemy
{
    public float minSpeed = 10;
    public float maxSpeed = 20;
    
    public float HP = 10.0f;
    public float maxHP = 10.0f;

    public AudioClip[] sounds;

    public Animator animator;

    public float power = 1;
    
    private float _speed = 12f;
    private Player target;

    private bool attacked = false;
    public bool Hit { private set; get; }
    private float attackTime = 1.2f;
    private float timer = 0;
    
    private float lastHitTime = 1f;
    private float timerHit = 0;

    private bool disabledScript = false;
    
    private Rigidbody rigidbodyy;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _speed = Random.Range(minSpeed, maxSpeed);
        
        rigidbodyy = GetComponent<Rigidbody>();
        
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (!disabledScript)
        {
            if (sounds.Length > 0 && _audioSource.isPlaying == false && Random.Range(0, 100) <= 50)
            {
                _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
                _audioSource.Play();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (disabledScript)
        {
            return;
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
        
        if (Hit)
        {
            timerHit += Time.deltaTime;

            if (timerHit >= lastHitTime)
            {
                Hit = false;
                timerHit = 0;
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

    public bool DealDamage(float damage)
    {
        if (Hit && damage > 0)
        {
            return false;
        }
        
        timerHit = 0;
        if (damage > 0)
        {
            HP -= damage;
            Hit = true;
        }

        if (HP <= 0)
        {
            StartCoroutine(Die());

            return true;
        }

        if (damage > 0)
        {
            animator.SetTrigger("TakeDamage");
        }

        rigidbodyy.AddForce(transform.forward * -400);
        rigidbodyy.AddForce(Vector3.up * 300);

        return false;
    }
    
    public IEnumerator Die()
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
