using UnityEngine;
using System.Collections;


/// <summary>State where the client will connect to the server.</summary>
public class ConnectionState : State
{
    /// <summary>True if level is loaded.</summary>
    protected bool loaded;

    /// <summary>The UI displayed during this state.</summary>
    private ConnectionMenu connectionMenu;


    /// <summary>Constructor.</summary>
    public ConnectionState(StateManager stateManager)
        : base(stateManager)
    {
        EventManager.AddListener(EnumEvent.AUTHSUCCEEDED, onConnectedToTeacher);
    }


    /// <summary>Called when the client is connected to the teacher. It includes TCP connection + authentication</summary>
    /// <returns>void</returns>
    public void onConnectedToTeacher()
    {
        EventManager.Raise(EnumEvent.CLOSEMENU);
        EventManager<string>.Raise(EnumEvent.LOADLEVEL, "QuestionAnswer");
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        connectionMenu = GameObject.FindGameObjectWithTag("ConnectionMenu").GetComponent<ConnectionMenu>();
        EventManager.Raise(EnumEvent.CONNECTIONSTATE);
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
    }


    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key)
    {
        if(loaded)
        {
            if (key == EnumInput.TAB)
                connectionMenu.switchUiSelect();
            else if (key == EnumInput.RETURN)
                connectionMenu.onConnectionStartClick();
        }
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
