public abstract class State
{

    public FSM owner;

    public State(FSM owner)
    {
        this.owner = owner;
    }

    public abstract string Description();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}

