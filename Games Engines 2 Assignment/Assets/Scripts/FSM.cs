using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Boid))]
public class FSM : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;

    State state = null;

    // Use this for initialization
    void Start () {
	
	}

    public void SwitchState(State state)
    {
        if (this.state != null)
        {
            this.state.Exit();
        }

        this.state = state;

        if (this.state != null)
        {
            this.state.Enter();
        }
    }

    // Update is called once per frame
    void Update () {
        if (state != null)
        {
            state.Update();
        }
    }
}
