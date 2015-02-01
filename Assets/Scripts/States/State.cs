using UnityEngine;
using System.Collections;

/// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
/// <param name="key">The input sent.</param>
/// <returns>void</returns>
public abstract class State {

    /// <summary>True if the level is loaded.</summary>
    protected bool loaded;

    /// <summary>Reference to the state manager.</summary>
    protected StateManager stateManager_;

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public abstract void start();

    /// <summary>Called when leaving this state.</summary>
    /// <returns>void</returns>
    public abstract void end();

    /// <summary>Called each frame.</summary>
    /// <returns>void</returns>
    public abstract void update();

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public abstract void onLevelWasLoaded(int lvl);

    /// <summary>Constructor.</summary>
    public State(StateManager stateManager)
    {
        stateManager_ = stateManager;
	}

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public abstract void noticeInput(EnumInput key);

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public abstract void noticeInput(EnumInput key, Touch[] inputs);
}
