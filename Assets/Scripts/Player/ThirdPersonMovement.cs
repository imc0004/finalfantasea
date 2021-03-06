﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour 
{
    public Transform cameraTransform;
    public Transform player;
    public Rigidbody rb;

    public float speed = 0.1f;
    public float turnSmoothing = 0.1f;

    private Quaternion rotation; 
    private bool inputDetected = false;

    float turnOmega;

    private void Awake() {
        //rigidbody = GetComponent<Rigidbody>();
    }

    Vector3 getInputDir()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }

    void FixedUpdate() {
        if (inputDetected) {
            Vector3 moveDir = rotation * Vector3.forward;
            Vector3 normMoveDir = moveDir.normalized;
            normMoveDir.y = 0f;
            rb.MovePosition(rb.transform.position + (normMoveDir * speed * Time.fixedDeltaTime));
            rb.MoveRotation(rotation);
        }
    }

    void Update()
    {
        Vector3 direction = getInputDir();
        if (direction.magnitude >= 0.1f) {
            inputDetected = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnOmega, turnSmoothing);
            rotation = Quaternion.Euler(0f, angle, 0f); 
        } else {
            inputDetected = false;
        }
    }
}
