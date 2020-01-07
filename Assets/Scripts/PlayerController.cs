﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float turnSpeed = 5f;

    private CharacterController characterController;
    
    [SerializeField]
    private Transform lookAtTransform;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        PlayerMovement(horizontal, vertical);
    }


    public void PlayerMovement(float horizontal, float vertical)
    {
        var movement = new Vector3(0, 0, 0);

        var xVector = new Vector3();
        var zVector = new Vector3();
        if (vertical == 1)
        {
            xVector = lookAtTransform.forward;
        }
        if (vertical == -1)
        {
            xVector = -lookAtTransform.forward;
        }
        if (horizontal == 1)
        {
            zVector = lookAtTransform.right;
        }
        if (horizontal == -1)
        {
            zVector = -lookAtTransform.right;
        }
        movement = xVector + zVector;

        movement.y = 0;
        movement.Normalize();

        characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);

        if (movement.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
        }
    }
}
