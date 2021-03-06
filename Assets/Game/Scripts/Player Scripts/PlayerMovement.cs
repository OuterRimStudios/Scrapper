﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float movementSpeed;
    public float rotationSpeed;
    public float animationSmoothing = 10f;

    [Space, Header("Jump Variables")]
    public float jumpForce = 8f;
    public float gravity = 20f;
    public float distToGround = 1.1f;
    public LayerMask groundLayer;

    [Space, Header("Mobility Variables")]
    public float sprintSpeed;
    public float hoverForce;

    public static bool canMove;
    public static bool canRotate;

    public Vector3 Direction { get; private set; }
    
    bool isCharging;
    Vector3 chargePosition;
    float chargeSpeed;

    float speed;

    float moveX;
    float moveY;

    float lookX;
    float lookY;

    float xRotationValue;

    float jump;

    bool isJumping;
    bool isSprinting;
    [HideInInspector]
    public bool hovering;

    Camera myCamera;
    Vector3 movement;
    Quaternion rotation;
    CharacterController cc;
    Animator anim;

    private void Start()
    {
        canMove = true;
        canRotate = true;
        anim = GetComponentInChildren<Animator>();
        speed = movementSpeed;
        myCamera = Camera.main;

        cc = GetComponent<CharacterController>();
    }

    public void RecieveInput(float _moveX, float _moveY, float _lookX, float _lookY)
    {
        moveX = _moveX;
        moveY = _moveY;
        lookX = _lookX;
        lookY = _lookY;
    }

    void Animate()
    {
        anim.SetFloat("MoveX", moveX, 1f, animationSmoothing * Time.deltaTime);
        anim.SetFloat("MoveY", moveY, 1f, animationSmoothing * Time.deltaTime);
        anim.SetFloat("LookX", lookX, 1f, animationSmoothing * Time.deltaTime);
        anim.SetFloat("LookY", myCamera.transform.rotation.x, 1f, animationSmoothing * Time.deltaTime);

        if (movement == Vector3.zero)
            anim.SetBool("IsMoving", false);
        else
            anim.SetBool("IsMoving", true);

        if(Grounded())
            anim.SetBool("IsJumping", false);
    }

    void ResetMovementAnimations()
    {
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);
        anim.SetBool("IsMoving", false);
    }

    void ResetLookAnimations()
    {
        anim.SetFloat("LookX", 0);
        anim.SetFloat("LookY", 0);
    }

    private void Update()
    {
        if (!canMove && !canRotate)
        {
            ResetMovementAnimations();
            ResetLookAnimations();
            return;
        }

        if (canRotate)
            Look();
        else
            ResetLookAnimations();

        if (canMove)
        {
            if (isCharging)
            {
                if (Utility.CheckDistance(transform.position, chargePosition) > 1f)
                    transform.position = Vector3.Lerp(transform.position, chargePosition, chargeSpeed * Time.deltaTime);
                else
                {
                    chargePosition = Vector3.zero;
                    isCharging = false;
                }
                return;
            }
            Move();
            Jumping();
            Animate();
        }
        else
        {
            ResetMovementAnimations();
        }
    }

    void Move()
    {
        movement = new Vector3(moveX, 0, moveY);
        Direction = movement;
        Vector3.ClampMagnitude(movement, 1);
        movement *= speed * Time.deltaTime;
        movement = myCamera.transform.TransformDirection(movement);
        cc.Move(movement);
    }

    void Look()
    {
        xRotationValue += lookX * rotationSpeed * Time.deltaTime;

        rotation = Quaternion.Euler(0, xRotationValue, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }

    void Jumping()
    {
        cc.Move(new Vector3(0, jump, 0));

        if(!hovering)
            jump -= gravity * Time.deltaTime;
        else
            jump -= (gravity * .5f) *Time.deltaTime;

        if (isJumping && Grounded())
        {
            isJumping = false;
        }
    }

    public void Jump()
    {
        if (!Grounded() && !hovering) return;

        anim.SetBool("IsJumping", true);

        if(!hovering)
            jump = jumpForce;
        else
            jump = hoverForce;
        isJumping = true;
    }

    public void Jump(float force)
    {
        anim.SetBool("IsJumping", true);
        jump = force;
        isJumping = true;
    }

    public void Hover(bool hover)
    {
        hovering = hover;
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayer);
    }

    public void Sprint(bool _isSprinting)
    {
        isSprinting = _isSprinting;
        if (isSprinting)
            speed = sprintSpeed;
        else
            speed = movementSpeed;
    }

    public void Charge(Vector3 _chargePosition, float _chargeSpeed)
    {
        isCharging = true;
        chargePosition = _chargePosition;
        chargeSpeed = _chargeSpeed;
    }
}
