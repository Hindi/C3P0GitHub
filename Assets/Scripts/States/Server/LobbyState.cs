using UnityEngine;
using System.Collections;

/// <summary>The state where the server is on start.</summary>
public class LobbyState : State {


    /// <summary>Constructor.</summary>
    public LobbyState(StateManager stateManager) : base(stateManager)
    {

    }

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
    {
        C3PONetwork.Instance.createTeacherServer();
        EventManager.Raise(EnumEvent.SERVERUI);
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
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
        /* TODO remplacer par le GUI */
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
        QuestionManager.Instance.update();
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
