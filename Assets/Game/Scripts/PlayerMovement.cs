using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float movementSpeed;
    public float rotationSpeed;

    [Space, Header("Jump Variables")]
    public float jumpForce = 8f;
    public float gravity = 20f;
    public float distToGround = 1.1f;
    public LayerMask groundLayer;

    float speed;

    float moveX;
    float moveY;

    float lookX;
    float lookY;

    float xRotationValue;

    float jump;

    bool isJumping;

    Camera myCamera;
    Vector3 movement;
    Quaternion rotation;
    CharacterController cc;

    private void Start()
    {
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

    private void Update()
    {
        Move();
        Look();
        Jumping();
    }

    void Move()
    {
        movement = new Vector3(moveX, 0, moveY).normalized;
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
        jump -= gravity * Time.deltaTime;

        if (isJumping && Grounded())
        {
            isJumping = false;
        }
    }

    public void Jump()
    {
        if (!Grounded()) return;

        jump = jumpForce;
        isJumping = true;
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayer);
    }
}
