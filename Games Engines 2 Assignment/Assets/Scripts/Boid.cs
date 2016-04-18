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

    [Header("Formation Following")]
    public bool formationFollowingEnabled = false;
    public GameObject formationLeader;
    public Vector3 formationOffset = Vector3.zero;
    public Vector3 formationTarget;

    [Header("Pursue")]
    public bool pursueEnabled;
    public GameObject pursueTarget;
    public Vector3 pursueTargetPos;

    [Header("Patrolling")]
    public bool patrolEnabled;
    public Transform patrolTransform;
    public float patrolRadius;
    public Vector3 patrolTarget = Vector3.zero;

    void Update() {
        force = Vector3.zero;

        if (seekEnabled) {
            force += Seek(seekTargetPos);
        }
        if (arriveEnabled) {
            force += Arrive(arriveTargetPos);
        }
        if (fleeEnabled) {
            force += Flee(fleeTargetPos, fleeRange);
        }
        if (formationFollowingEnabled) {
            force += Formation(formationLeader, formationOffset);
        }
        if (pursueEnabled) {
            force += Pursue(pursueTarget);
        }
        if (patrolEnabled) {
            force += Patrol();
        }

        force = Vector3.ClampMagnitude(force, maxForce);

        Vector3 acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity.magnitude > float.Epsilon) {
            if (velocity != Vector3.zero) {
                transform.forward = velocity;
            }
        }
        // Change the pitch of the sound
        UpdatePitch();
        velocity *= (1.0f - damping);
    }

    public void TurnOffAll() {
        seekEnabled = arriveEnabled = fleeEnabled =
            formationFollowingEnabled = patrolEnabled = pursueEnabled = false;
    }

    Vector3 Formation(GameObject leader, Vector3 offset) {
        if (leader != null && formationOffset != Vector3.zero) {
            formationTarget = leader.transform.position + offset;
            return Arrive(formationTarget);
        }
        else {
            return Vector3.zero;
        }
    }

    Vector3 Patrol() {
        if (patrolTarget == Vector3.zero) {
            patrolTarget = MakeNextWaypoint();
        }
        if (Vector3.Distance(patrolTarget, transform.position) < 1.0f) {
            patrolTarget = MakeNextWaypoint();
        }
        return Seek(patrolTarget);
    }

    Vector3 MakeNextWaypoint() {
        if (patrolTransform != null) {
            float angle = Random.Range(0, 360);
            float x = patrolTransform.position.x + (patrolRadius * Mathf.Cos(angle * Mathf.Deg2Rad));
            float y = patrolTransform.position.z + (patrolRadius * Mathf.Sin(angle * Mathf.Deg2Rad));
            return new Vector3(x, 0, y);
        }
        else {
            return Vector3.zero;
        }
    }

    Vector3 Seek(Vector3 target) {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }

    public Vector3 Arrive(Vector3 targetPos) {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 15.0f;
        float distance = toTarget.magnitude;
        if (distance < 0.5f) {
            velocity = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }

    public Vector3 Flee(Vector3 targetPos, float range) {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        if (desiredVelocity.magnitude > range) {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        return desiredVelocity - velocity;
    }

    public Vector3 Pursue(GameObject target) {
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAhead = toTarget.magnitude / maxSpeed;
        pursueTargetPos = target.transform.position
           + (target.GetComponent<Boid>().velocity * lookAhead);

        return Seek(pursueTargetPos);
    }

    public void UpdatePitch() {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null) {
            audio.pitch = velocity.magnitude / maxSpeed + 0.5f;
        }
    }
}
