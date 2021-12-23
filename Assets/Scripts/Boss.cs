using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    public float HP = 10.0f;
    public float maxHP = 10.0f;

    public AudioClip startSound;
    public AudioClip[] sounds;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip winSound;

    public Animator animator;
    
    public bool Hit { private set; get; }
    
    private Player target;
    private AudioSource _audioSource;
    
    private bool attacked = false;
    private float attackTime = 4f;
    private float timer = 0;
    
    private float lastHitTime = 1f;
    private float timerHit = 0;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    private void OnEnable()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        _audioSource.clip = startSound;
        _audioSource.Play();
    }

    private void Update()
    {
        //Strange bug
        animator.transform.localPosition = new Vector3(0f, -2.13f, 0f);

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
        
        if (Hit)
        {
            _audioSource.clip = hitSound;
            _audioSource.Play();
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
        transform.LookAt(target.transform.position);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    public void CastSpell()
    {
        if (sounds.Length > 0 && _audioSource.isPlaying == false && Random.Range(0, 100) < 50)
        {
            _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            _audioSource.Play();
        }
        
        attacked = true;
        timer = 0;
        
        animator.SetTrigger("Attack" + Random.Range(1, 2));
    }
    
    bool IEnemy.DealDamage(float damage)
    {
        if (Hit && damage > 0)
        {
            return false;
        }
        
        timerHit = 0;
        if (damage > 0)
        {
            _audioSource.clip = hitSound;
            _audioSource.Play();
            
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
            animator.SetTrigger("Hit");
        }
        
        return false;
    }
    
    private IEnumerator Die()
    {
        target.UiManager.OpenSummaryPanel();
        
        _audioSource.clip = dieSound;
        _audioSource.Play();

        yield return new WaitForSeconds(1.5f);
        
        _audioSource.clip = winSound;
        _audioSource.Play();
        
        yield return new WaitForSeconds(4);
    }
}
