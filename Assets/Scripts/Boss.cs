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
    public Transform spellCastPoint;

    public Animator animator;
    public Spell spellPrefab;
    
    public bool Hit { private set; get; }
    
    private Player target;
    private AudioSource _audioSource;
    
    private bool attacked = false;
    private float attackTime = 3f;
    private float timer = 0;
    
    private float lastHitTime = 1f;
    private float timerHit = 0;

    private bool disabledScript;

    private List<Spell> spells;

    private Quaternion _lastRotation;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        spells = new List<Spell>();
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
        if (disabledScript)
        {
            return;
        }

        if (spells.Count > 10)
        {
            spells.RemoveAt(0);
        }

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
        else StartCoroutine(CastSpell());
        
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
        if (disabledScript)
        {
            return;
        }
        
        transform.LookAt(target.transform.position);
        _lastRotation = transform.rotation;
        
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    public IEnumerator CastSpell()
    {
        if (sounds.Length > 0 && _audioSource.isPlaying == false && Random.Range(0, 100) < 20)
        {
            _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            _audioSource.Play();
        }
        
        attacked = true;
        timer = 0;
        
        animator.SetTrigger("Attack" + Random.Range(1, 2));

        yield return new WaitForSeconds(0.5f);

        var nextSpell = Instantiate(spellPrefab, spellCastPoint.position, _lastRotation);
        nextSpell.target = target;
        
        spells.Add(nextSpell);
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
            HP -= damage;
            Hit = true;
        }

        if (HP <= 0)
        {
            _audioSource.clip = dieSound;
            _audioSource.Play();
            
            StartCoroutine(Die());

            return true;
        }
        else if (damage > 0)
        {
            _audioSource.clip = hitSound;
            _audioSource.Play();
        }

        if (damage > 0)
        {
            animator.SetTrigger("Hit");
        }
        
        return false;
    }
    
    private IEnumerator Die()
    {
        //die animation and dispose all enemies

        yield return new WaitForSeconds(5);
        
        _audioSource.clip = winSound;
        _audioSource.Play();
        
        target.UiManager.OpenSummaryPanel(true, target.score);
    }
}
