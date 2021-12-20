using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float HP = 100.0f;
    public float maxHP = 100.0f;
    public GameObject attackPoint;
    public LayerMask enemyLayer;

    public Animator animator;
    public HudManager hudManager;

    public Canvas canvas;
    public TextMeshProUGUI textScore;

    private bool isGround = true;

    private Rigidbody rigidbodyy;
    private float horizontalMove;
    private float verticalMove;

    private Vector3 offset;
    private float score;
    private Quaternion lookRotation;

    private int step = 0;
    private float timer = 0;
    private float attacktime = 1;
    private float velocity;
    
    private enum State
    {
        Idle, Action
    }

    private State _state = State.Idle;
    
    private void Start() {
        rigidbodyy = GetComponent<Rigidbody>();
        canvas.gameObject.SetActive(false);
        
        isGround = true;
        score = 0;
        animator.SetBool("IsGround", true);
        
        Time.timeScale = 1.0f;
    }

    private void Update() {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        
        velocity = speed * verticalMove;
        animator.SetFloat("Speed", velocity);
        Debug.Log(verticalMove);

        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= attacktime)
        {
            StartCoroutine(Attack());
            
            timer = 0;
        }

        bool isClickedJump = Input.GetKeyUp("space");

        score += (1 * step) * Time.deltaTime;
        textScore.text = $"Punkty: {score:0.0}";
        
        hudManager.SetScore(score);
        hudManager.SetHP(HP, maxHP);

        if (isGround && isClickedJump) {
            isGround = false;
            _state = State.Action;
            
            animator.SetBool("IsGround", false);
            
            rigidbodyy.AddForce(Vector3.up * 300);
        }
    }
    
    private void FixedUpdate() {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenHalfSize = new Vector2(Screen.width, Screen.height) / 2f;

        if (_state == State.Idle)
        {
            var offset = 300f;
            var mousePosition = (mousePos - screenHalfSize);
            var mouseDirection = (mousePos - screenHalfSize).normalized;

            if (mousePosition.x > -screenHalfSize.x + offset && mousePosition.x < screenHalfSize.x - offset)
            {
                mouseDirection.x = 0;
            }

            var rotationAngle = mouseDirection.x * rotationSpeed * Time.fixedDeltaTime;
            
            transform.Rotate(transform.rotation.x, rotationAngle, transform.rotation.z);
            transform.position += velocity * transform.forward * Time.fixedDeltaTime;
        }
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
        canvas.gameObject.SetActive(true);
        canvas.GetComponent<GameplayMenuManager>().Init((int)score);

        Time.timeScale = 0.0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") || isGround)
        {
            return;
        }

        isGround = true;
        _state = State.Idle;
        
        animator.SetBool("IsGround", true);
    }
}
