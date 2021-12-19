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
    
    private void Start() {
        rigidbodyy = GetComponent<Rigidbody>();
        canvas.gameObject.SetActive(false);
        
        isGround = true;
        score = 0;
        animator.SetBool("IsGround", true);
        
        Time.timeScale = 1.0f;
    }

    private void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        
        velocity = speed * verticalMove;
        animator.SetFloat("Speed", velocity);
        Debug.Log(verticalMove);

        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= attacktime)
        {
            animator.SetTrigger("Attack");
            timer = 0;
        }

        bool isClickedJump = Input.GetKeyUp("space");

        score += (1 * step) * Time.deltaTime;
        textScore.text = $"Punkty: {score:0.0}";
        
        hudManager.SetScore(score);
        hudManager.SetHP(HP, maxHP);

        if (isGround && isClickedJump) {
            isGround = false;
            
            animator.SetBool("IsGround", false);
            
            rigidbodyy.AddForce(Vector3.up * 300);
        }
    }
    
    private void FixedUpdate() {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenHalfSize = new Vector2(Screen.width, Screen.height) / 2f;

        var mouseDirection = (mousePos - screenHalfSize).normalized;
        if (mouseDirection.x <= 0.9 && mouseDirection.x >= -0.9) {
            mouseDirection.x = 0;
        }

        transform.Rotate(transform.rotation.x, mouseDirection.x * rotationSpeed * Time.fixedDeltaTime, transform.rotation.z);
        transform.position += velocity * transform.forward * Time.fixedDeltaTime;
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
        
        animator.SetBool("IsGround", true);
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
