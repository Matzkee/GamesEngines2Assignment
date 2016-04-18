using UnityEngine;
using System.Collections;


public class PatrollingState : State
{
    public PatrollingState(FSM owner) : base(owner)
    {
    }

    public override string Description()
    {
        return "Patrolling/Defending";
    }

    public override void Enter()
    {
        owner.GetComponent<Boid>().patrolTransform = owner.motherShip.transform;
        owner.GetComponent<Boid>().patrolRadius = owner.patrolRadius;
        owner.GetComponent<Boid>().patrolEnabled = true;
    }

    public override void Exit()
    {
        owner.GetComponent<Boid>().patrolEnabled = false;
    }

    public override void Update()
    {
        
    }
}