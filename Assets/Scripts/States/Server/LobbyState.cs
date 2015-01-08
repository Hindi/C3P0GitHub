using UnityEngine;
using System.Collections;

public class LobbyState : State {

    public LobbyState(StateManager stateManager) : base(stateManager)
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }

    public void start()
    {
        C3PONetwork.Instance.createTeacherServer();
        EventManager.Raise(EnumEvent.SERVERUI);
    }

    public void onLevelWasLoaded(int lvl)
    {

    }

    public void end()
    {

    }

    public void update()
    {
        /* TODO remplacer par le GUI */
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
    }


}
