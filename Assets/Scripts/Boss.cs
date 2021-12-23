using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    public float HP = 10.0f;
    public float maxHP = 10.0f;

    public AudioClip[] sounds;

    public Animator animator;
    
    private Player target;
    private AudioSource _audioSource;
    
    private bool attacked = false;
    private float attackTime = 2f;
    private float timer = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
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
        else CastSpell();
    }

    private void FixedUpdate()
    {
        transform.LookAt(target.transform.position);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    public void CastSpell()
    {
        attacked = true;
        timer = 0;
        
        animator.SetTrigger("Attack" + Random.Range(1, 2));
    }
    
    bool IEnemy.DealDamage(float damage)
    {
        if (damage > 0)
        {
            HP -= damage;
        }

        if (HP <= 0)
        {
            StartCoroutine(Die());

            return true;
        }

        if (damage > 0)
        {
            animator.SetTrigger("Hit");
        }
        
        return false;
    }
    
    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;

        yield return new WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }
}
