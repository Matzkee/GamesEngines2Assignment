using UnityEngine;
using System.Collections;

public class HyperJump : MonoBehaviour {

    public Vector3 hyperJumpPoint;
    Quaternion jumpDirection;
    Boid boid;
    public float speed = 200;
    public float rotationSpeed = 5;
    bool turning = false;
    bool completedRotating = false;
    bool arrived = false;
    bool jumped = false;
    float scale = 1.0f;
    public float scaleRatio = 0.999f;

    // Use this for initialization
    void Start () {
        boid = GetComponent<Boid>();

        Prepare();
	}
	
	// Update is called once per frame
	void Update () {
        if (!arrived && Vector3.Distance(transform.position, hyperJumpPoint) < 1f)
        {
            boid.arriveEnabled = false;
            arrived = true;
            turning = true;
        }
        if (turning)
        {
            turnToJump();
        }
        if (arrived && completedRotating)
        {
            if (!jumped)
            {
                hyperJump();
            }
        }
    }

    void Prepare()
    {
        Vector3 toHyperJump = hyperJumpPoint + Vector3.forward;
        jumpDirection = Quaternion.LookRotation(toHyperJump - hyperJumpPoint, Vector3.up);
        boid.arriveEnabled = true;
        boid.arriveTargetPos = hyperJumpPoint;
    }

    void turnToJump()
    {
        float angle = Quaternion.Angle(transform.rotation, jumpDirection);
        if (angle > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, jumpDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            turning = false;
            completedRotating = true;
        }
    }

    void hyperJump()
    {
        if (!jumped)
        {
            if (scale <= 0.1)
            {
                jumped = true;
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                transform.localScale *= scaleRatio;
                scale *= 0.95f;
            }
        }
    }
}
