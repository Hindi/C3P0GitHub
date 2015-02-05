using UnityEngine;
using System.Collections;

/// <summary>This state is reached only when the game starts. It is used for initialisation.</summary>
class InitState : State {

    /// <summary>Constructor.</summary>
    public InitState(StateManager stateManager) : base(stateManager)
    {

    }

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
    {
	}

    /// <summary>Called when leaving this state.</summary>
    /// <returns>void</returns>
    public override void end()
    {

    }

    /// <summary>Called each frame.</summary>
    /// <returns>void</returns>
    public override void update()
    {
        if (C3PONetwork.Instance.IS_SERVER)
        {
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "ServerLobby");
        }
        else
        {
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
        }
	}

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key)
    {

    }

    /// <summary>Recieves all the necessary inputs (touchscreen & mobile phone buttons).</summary>
    /// <param name="key">The input sent.</param>
    /// <param name="inputs">Array containing the touch inputs.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
