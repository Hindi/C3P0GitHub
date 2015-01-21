using UnityEngine;
using System.Collections;


class InitState : State {

    float startTime;
    
    public InitState(StateManager stateManager) : base(stateManager)
    {
        startTime = Time.time;
    }

	// Use this for initialization
    public void start()
    {

	}

    // Use this for state transition
    public override void end()
    {

    }
	
	// Update is called once per frame
    public override void update()
    {
        if (Time.time - startTime > 0.1)
		{
            if (C3PONetwork.Instance.IS_SERVER)
            {
                stateManager_.changeState(StateEnum.SERVERCONNECTION);
                EventManager<string>.Raise(EnumEvent.LOADLEVEL, "ServerLobby");
            }
            else
            {
                stateManager_.changeState(StateEnum.CONNECTION);
                EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
            }
        }
	}

    public override void noticeInput(KeyCode key)
    {

    }
}
