using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    public Animator animator;

    public ObstacleGenerator obstacleGenerator;

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
    
    void Start() {
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

        bool isClickedJump = Input.GetKeyUp("space");

        score += (1 * step) * Time.deltaTime;
        textScore.text = $"Punkty: {score:0.0}";

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

        Debug.Log($"transform: {transform.forward}");
        var velocity = Time.fixedDeltaTime * speed * verticalMove * transform.forward;
        Debug.Log($"velocity: {velocity}");
        transform.position += velocity;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Obstacle")) {
            canvas.gameObject.SetActive(true);
            canvas.GetComponent<GameplayMenuManager>().Init((int)score);

            Time.timeScale = 0.0f;

            return;
        }

        if (isGround)
            return;

        isGround = true;
        
        animator.SetBool("IsGround", true);
    }
}
