using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    
    [SerializeField]
    private float speed = 6f;
    private float sprintSpeed = 7f; // new variable for sprint speed
    private bool isDrunk = true;
    
    private Animator animator;
    
    private float gravity = -9.81f;
    private float jumpHeight = 2f;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    Vector3 velocity;
    bool isGrounded;

    
    void Start()
    {
        //animator = GetComponentInChildren<Animator>(true);    
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        
        if (Input.GetKey(KeyCode.LeftShift)) // check if Left Shift is pressed
        {
            move *= sprintSpeed; // if yes, use sprint speed
        }
        else
        {
            move  *= speed; // if not, use regular speed
        }
        
        if(move != Vector3.zero)
        {
            if (isDrunk)
            {
                animator.SetBool("isMovingDrunk", true);

            }
            else
            {
                animator.SetBool("isMoving", true);

            }
            
        }
        else
        {
            if (isDrunk)
            {
                animator.SetBool("isMovingDrunk", false);

            }
            else
            {
                animator.SetBool("isMoving", false);

            }
        }
        
        controller.Move(move *  Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump");
            animator.SetBool("Jumping", true);

            velocity.y = Mathf.Sqrt( -2f * gravity);
        
        }

        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
    }
}
