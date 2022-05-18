using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attributes:")]
    [SerializeField] int baseSpeed = 5;

    [Space]
    [Header("References:")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    float moveModifyer = 1f;
    float moveSpeed;
    Vector2 moveDirection;
    
    void Update()
    {
        HandleInput();
        Move();
        Animate();
    }

    void HandleInput()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveSpeed = Mathf.Clamp(moveDirection.magnitude, 0.0f, 1.0f);
        moveDirection.Normalize();
    }

    void Move()
    {
        rb.velocity = moveDirection * moveSpeed * baseSpeed;
    }

    void Animate()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
        }

        animator.SetFloat("Speed", moveSpeed);
    }
}
