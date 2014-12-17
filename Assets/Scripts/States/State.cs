using UnityEngine;
using System.Collections;

/**
 * The abstract class from which the states are inherited.
 * 
 */

public abstract class State {

    protected StateManager stateManager_;

    // Use this for state transition
    public virtual void start()
    {

    }
        
    // Use this for state transition
    public virtual void end()
    {

    }

    // Update is called once per frame
    public virtual void update()
    {

    }

	public virtual void onLevelWasLoaded(int lvl)
	{

	}

	// Use this for initialization
    public State(StateManager stateManager)
    {
        stateManager_ = stateManager;
	}

    public abstract void noticeInput(KeyCode key);
}
