using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class manage the states in the whole game (menus and minigame)
 * 
 */

public class StateManager : MonoBehaviour {

    [SerializeField]    //For debug
    private State currentState;

    [SerializeField]
    private Dictionary<StateEnum, State> stateList;

	// Use this for initialization
	void Start () {
        stateList = new Dictionary<StateEnum, State>();
        currentState = new InitState(this);

        stateList.Add(StateEnum.LUNARLANDER, new LunarLanderState(this));
        stateList.Add(StateEnum.SPACEINVADER, new SpaceInvaderState(this));
        stateList.Add(StateEnum.PONG, new PongState(this));
        stateList.Add(StateEnum.CONNECTION, new ConnectionState(this));
        stateList.Add(StateEnum.QUESTIONANSWER, new QuestionAnswerState(this));
        stateList.Add(StateEnum.SERVERCONNECTION, new LobbyState(this));
        stateList.Add(StateEnum.TETRIS, new TetrisState(this));
        stateList.Add(StateEnum.SPACEWAR, new SpaceWarState(this));
        currentState.start();
	}
	
	// Update is called once per frame
	void Update () {
        currentState.update();
	}

	
	public void OnLevelWasLoaded(int lvl)
	{
		currentState.onLevelWasLoaded (lvl);
	}

    public void noticeInput(EnumInput key)
    {
        currentState.noticeInput(key);
    }

    public void noticeInput(EnumInput key, Touch[] inputs)
    {
        currentState.noticeInput(key, inputs);
    }

    public void changeState(StateEnum state)
    {
        currentState.end();
        currentState = stateList[state];
        currentState.start();
    }

    public void setParameter(Parameter param)
    {
        //On s'assure avant qu'on a bien un GameState
        if (typeof(GameState).IsAssignableFrom(currentState.GetType()))
            ((GameState)currentState).setParameter(param);
    }
}
