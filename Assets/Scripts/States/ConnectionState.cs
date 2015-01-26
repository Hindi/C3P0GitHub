using UnityEngine;
using System.Collections;

public class ConnectionState : State
{
    protected bool loaded;
    private GameObject networkManager;
    private ConnectionMenu connectionMenu;

    public ConnectionState(StateManager stateManager)
        : base(stateManager)
    {
        EventManager.AddListener(EnumEvent.AUTHSUCCEEDED, onConnectedToTeacher);
    }

    public void onConnectedToTeacher()
    {
        EventManager.Raise(EnumEvent.CLOSEMENU);
        EventManager<string>.Raise(EnumEvent.LOADLEVEL, "QuestionAnswer");
    }

    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        EventManager.Raise(EnumEvent.CONNECTIONSTATE);
    }

    // Use this for initialization
    public override void start()
    {

    }

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

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
