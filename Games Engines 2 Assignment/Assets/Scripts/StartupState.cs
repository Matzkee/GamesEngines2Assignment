using UnityEngine;
using System.Collections;


public class StartupState : State
{
    float liftOffDistance = 300.0f;
    Vector3 liftOff;
    public StartupState(FighterStateMachine owner) : base(owner)
    {
    }

    public override string Description()
    {
        return "Starting Engines";
    }

    public override void Enter()
    {
        liftOff = owner.transform.position;
        liftOff += owner.transform.forward * liftOffDistance;

        owner.GetComponent<Boid>().seekTargetPos = liftOff;
        owner.GetComponent<Boid>().seekEnabled = true;
    }

    public override void Exit()
    {
        owner.GetComponent<Boid>().seekEnabled = false;
    }

    public override void Update()
    {
        if (Vector3.Distance(owner.transform.position, liftOff) <= 5.0f)
        {
            owner.ready = true;
            if (owner.isCaptain) {
                owner.SwitchState(new PatrollingState(owner));
            }
            else {
                owner.SwitchState(new FormationFollowState(owner));
            }
        }
    }
}