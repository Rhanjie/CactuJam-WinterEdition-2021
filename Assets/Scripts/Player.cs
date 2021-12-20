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

    public Transform camera;
    public Animator animator;
    public UiManager UiManager;

    public Canvas canvas;
    public TextMeshProUGUI textScore;

    private bool isGround = true;

    private Rigidbody _rigidbody;
    private CharacterController _characterController;
    private float horizontalMove;
    private float verticalMove;

    private Vector3 offset;
    private float score;
    private Quaternion lookRotation;

    private int step = 0;
    private float timer = 0;
    private float attacktime = 1;

    private float gravity = -9.81f;
    private float _turnSmoothVelocity;
    
    private Vector3 velocity;

    private HudManager _hudManager;

    private Vector3 _lastSavedVelocity;
    
    private enum State
    {
        Idle, Action
    }

    private State _state = State.Idle;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();
        _hudManager = UiManager.GetHudManager();

        isGround = true;
        score = 0;
        animator.SetBool("IsGround", true);
        
        Time.timeScale = 1.0f;
    }

    private void Update() {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        
        isGround = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        
        var clipName = currentClipInfo[0].clip.name;
        _state = (clipName == "Idle" || clipName == "Running") ? State.Idle : State.Action;

        animator.SetBool("IsGround", isGround);
        
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2 * gravity);
            _state = State.Action;
        }
        
        velocity.y += gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);

        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= attacktime)
        {
            StartCoroutine(Attack());
            
            timer = 0;
        }

        bool isClickedJump = Input.GetKeyUp("space");

        score += (1 * step) * Time.deltaTime;
        
        _hudManager.SetScore(score);
        _hudManager.SetHP(HP, maxHP);
    }
    
    private void FixedUpdate()
    {
        /*if (_state == State.Action)
        {
            if (!isGround)
            {
                _characterController.Move(_lastSavedVelocity * Time.deltaTime);
            }
            return;
        }*/

        var direction = new Vector3(horizontalMove, 0f, verticalMove).normalized;
        animator.SetFloat("Speed", (direction * speed * Time.fixedDeltaTime).magnitude);
        
        if (direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _lastSavedVelocity = moveDir.normalized * speed;
            _characterController.Move(_lastSavedVelocity * Time.deltaTime);
        }
        
        else _lastSavedVelocity = Vector3.zero;
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        _state = State.Action;

        yield return new WaitForSeconds(0.5f);

        var hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, 1.2f, enemyLayer);

        foreach (var enemyCollider in hitEnemies)
        {
            if (!enemyCollider.isTrigger)
                continue;
            
            var enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy == null)
            {
                continue;
            }
            
            enemy.DealDamage(5);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        _state = State.Idle;
    }

    public void DealDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        UiManager.OpenSummaryPanel();

        Time.timeScale = 0.0f;
    }
}
