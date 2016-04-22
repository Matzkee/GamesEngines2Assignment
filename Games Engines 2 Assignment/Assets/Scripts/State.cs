public abstract class State
{

    public FighterStateMachine owner;

    public State(FighterStateMachine owner)
    {
        this.owner = owner;
    }

    public abstract string Description();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}

