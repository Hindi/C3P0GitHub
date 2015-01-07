using UnityEngine;
using System.Collections;

public class ConnectionState : State
{
    protected bool loaded;
    private GameObject networkManager;
    private C3PONetwork c3poNetwork;
    private C3PONetworkManager c3poNetworkManager;

    public ConnectionState(StateManager stateManager)
        : base(stateManager)
    {

    }

    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        c3poNetwork = networkManager.GetComponent<C3PONetwork>();
        c3poNetworkManager = networkManager.GetComponent<C3PONetworkManager>();
        EventManager.Raise(EnumEvent.CONNECTIONSTATE);
    }

    // Use this for initialization
    public override void start()
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }
}
