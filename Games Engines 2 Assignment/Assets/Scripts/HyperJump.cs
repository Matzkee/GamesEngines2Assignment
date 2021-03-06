﻿using UnityEngine;
using System.Collections;

public class HyperJump : MonoBehaviour {

    public Transform hyperJumpPoint;
    
    Quaternion jumpDirection;
    Boid boid;
    public float speed = 200;
    public float rotationSpeed = 5;
    public float jumpDelay = 15;

    bool readyToJump;

    bool turningInJumpDirection = false;
    bool completedRotating = false;
    bool jumped = false;
    float scale = 1.0f;
    public float scaleRatio = 0.999f;

    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();
        StartCoroutine("PrepareToJump");
	}
	
	// Update is called once per frame
	void Update () {
        if (turningInJumpDirection)
        {
            TurnToJump();
        }
        if (completedRotating)
        {
            Jump();
        }
    }

    void Prepare()
    {
        boid.TurnOffAll();
        jumpDirection = Quaternion.LookRotation(hyperJumpPoint.position - transform.position, Vector3.up);
    }

    void TurnToJump()
    {
        float angle = Quaternion.Angle(transform.rotation, jumpDirection);
        if (angle > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, jumpDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            turningInJumpDirection = false;
            completedRotating = true;
        }
    }

    void Jump()
    {
        if (!jumped)
        {
            if (scale <= 0.1)
            {
                jumped = true;
                gameObject.SetActive(false);
            }
            else
            {
                // rush forward without adding forces
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                scale *= scaleRatio;
            }
        }
    }

    IEnumerator PrepareToJump()
    {
        boid.arriveTargetPos = transform.position + transform.forward * speed;
        boid.arriveEnabled = true;
        // Wait 20 seconds and then jump
        while (!readyToJump)
        {

            readyToJump = true;
            yield return new WaitForSeconds(jumpDelay);
        }
        Prepare();
        // Start turning into jump direction
        turningInJumpDirection = true;

    }
}
