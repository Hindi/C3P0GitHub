﻿using UnityEngine;
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
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "SpaceInvader");
            stateManager_.changeState(StateEnum.SPACEINVADER);
        }
	}

    public override void noticeInput(KeyCode key)
    {

    }
}
