using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float damping;

    public float maxSpeed;
    public float maxForce;

    [Header("Seeking")]
    public bool seekEnabled = false;
    public Vector3 seekTargetPos;

    [Header("Arriving")]
    public bool arriveEnabled = false;
    public Vector3 arriveTargetPos;
    public float slowingDistance = 15;

    [Header("Fleeing")]
    public bool fleeEnabled = false;
    public Vector3 fleeTargetPos;
    public float fleeRange = 15.0f;

    void Update()
    {
        force = Vector3.zero;

        if (seekEnabled)
        {
            force += Seek(seekTargetPos);
        }
        if (arriveEnabled)
        {
            force += Arrive(arriveTargetPos);
        }
        if (fleeEnabled)
        {
            force += Flee(fleeTargetPos, fleeRange);
        }

        force = Vector3.ClampMagnitude(force, maxForce);

        Vector3 acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }

        velocity *= (1.0f - damping);
    }

    public void TurnOffAll()
    {
        seekEnabled = arriveEnabled = fleeEnabled = false;
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }

    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 15.0f;
        float distance = toTarget.magnitude;
        if (distance < 0.5f)
        {
            velocity = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }

    public Vector3 Flee(Vector3 targetPos, float range)
    {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        if (desiredVelocity.magnitude > range)
        {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        return desiredVelocity - velocity;
    }
}
