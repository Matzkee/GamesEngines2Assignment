using UnityEngine;
using System.Collections;
using System;

public class FormationFollowState : State {

    public FormationFollowState(FSM owner):base(owner)
    {

    }

    public override string Description()
    {
        return "Following Formation";
    }

    public override void Enter()
    {
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}
