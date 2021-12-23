using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpPower = 2;
    public float HP = 100.0f;
    public float maxHP = 100.0f;
    public GameObject attackPoint;
    public LayerMask enemyLayer;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask hangingMask;

    public Transform camera;
    public Animator animator;
    public UiManager UiManager;
    public AudioSource _hitAudioSource;
    
    public AnimTextManager animTextManager;
    public BossManager bossManager;
    
    public bool debugMode;

    private bool isGround = true;
    private bool isHanging = false;
    
    private float horizontalMove;
    private float verticalMove;

    private Vector3 offset;
    public int score { private set; get; }
    private Quaternion lookRotation;

    private int step = 0;
    private float timer = 0;
    private float attacktime = 1;

    private float gravity = -9.81f;
    private float _turnSmoothVelocity;
    
    private Vector3 velocity;

    private CharacterController _characterController;
    private AudioSource _audioSource;
    private HudManager _hudManager;

    private Vector3 _lastSavedVelocity;

    private Vector3 _lastDirection;
    
    private enum State
    {
        Idle, Action
    }

    private State _state = State.Idle;

    private String _lastClipName;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _hudManager = UiManager.GetHudManager();

        isGround = true;
        score = 0;
        animator.SetBool("IsGround", true);
    }

    private void Update() {
        if (_state == State.Idle)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");
        }

        else
        {
            horizontalMove = verticalMove = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UiManager.OpenMenuPanel();
        }
        
        bossManager.CheckLimit(score);

        isGround = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        if (!isGround)
        {
            isHanging = Physics.CheckSphere(transform.position, groundDistance, hangingMask);
        }
        else isHanging = false;
        
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        _lastClipName = currentClipInfo[0].clip.name;
        
        _state = (_lastClipName == "Idle" || _lastClipName == "Running" || _lastClipName == "Jump") ? State.Idle : State.Action;

        animator.SetBool("IsGround", isGround);
        animator.SetBool("isHanging", isHanging);

        if ((isGround || isHanging) && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && (isGround || isHanging) && _state == State.Idle)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2 * gravity);
        }
        
        velocity.y += gravity * (_state == State.Action ? 4 : 1) * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);

        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= attacktime && !isHanging)
        {
            StartCoroutine(Attack());
            
            timer = 0;
        }
        
        _hudManager.SetHP(HP, maxHP);
    }
    
    private void FixedUpdate()
    {
        if (_state == State.Action)
        {
            /*if (!isGround)
            {
                _characterController.Move(_lastSavedVelocity * Time.deltaTime);
            }*/
            //return;
        }

        _lastDirection = new Vector3(horizontalMove, 0f, verticalMove).normalized;
        animator.SetFloat("Speed", (_lastDirection * speed * Time.fixedDeltaTime).magnitude);
        
        if (_lastDirection.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(_lastDirection.x, _lastDirection.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _lastSavedVelocity = moveDir.normalized * speed;
            _characterController.Move(_lastSavedVelocity * Time.deltaTime);
        }
        
        else _lastSavedVelocity = Vector3.zero;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        _state = State.Action;

        if (!isGround)
        {
            velocity.y = 0;
        }

        /*if (_lastDirection.magnitude >= 0.1f)
        {
            animator.SetLayerWeight(1, 0.65f);
        }*/
        
        _hitAudioSource.Play();

        yield return new WaitForSeconds(0.5f);
        
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        _lastClipName = currentClipInfo[0].clip.name;

        do
        {
            var hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.3f, enemyLayer);

            var additionalScore = 0;
            foreach (var enemyCollider in hitEnemies)
            {
                if (!enemyCollider.isTrigger)
                    continue;

                var enemyBase = enemyCollider.GetComponent<IEnemy>();
                if (enemyBase == null)
                {
                    continue;
                }

                //Ignore hit enemy
                if (enemyBase is Enemy {Hit: true})
                {
                    continue;
                }

                var isDead = enemyBase.DealDamage(5);
                if (isDead)
                {
                    additionalScore = (additionalScore != 0)
                        ? additionalScore * 2
                        : 1;
                }
            }

            if (hitEnemies.Length > 0)
            {
                score += additionalScore;
                _hudManager.SetScore(score);
                animTextManager.PushText(additionalScore);
            }

            yield return new WaitForSeconds(0.05f);
            Debug.Log(_lastClipName);
            
        } while (_lastClipName == "Standing Melee Attack Downward");

        //while (!isGround && _state == State.Action);
    }

    public void DealDamage(float damage)
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        HP -= damage;

        if (HP <= 0 && !debugMode)
        {
            Die();
        }
    }

    private void Die()
    {
        UiManager.OpenSummaryPanel(false, score);
    }
}
