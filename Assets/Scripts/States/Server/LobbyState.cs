using UnityEngine;
using System.Collections;

public class LobbyState : State {

    public LobbyState(StateManager stateManager) : base(stateManager)
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }

    public override void start()
    {
        C3PONetwork.Instance.createTeacherServer();
        EventManager.Raise(EnumEvent.SERVERUI);
    }

    public override void onLevelWasLoaded(int lvl)
    {

    }

    public override void end()
    {

    }

    public override void update()
    {
        /* TODO remplacer par le GUI */
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
    }


}
