using UnityEngine;
using System.Collections;

/**
 * The main menu.
 * 
 */

public class MainMenuState : State {
    
    public MainMenuState(StateManager stateManager) : base(stateManager)
    {

    }

	// Use this for initialization
    public void start()
    {

	}

    // Use this for state transition
    public override void end()
    {
        Debug.Log("MENU OVER");
    }
	
	// Update is called once per frame
    public override void update()
    {
	    
	}

    public override void noticeInput(KeyCode key)
    {
        if (key == KeyCode.Return)
        {
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "StudLevel1");
            stateManager_.changeState(StateEnum.GAME);
        }
        if (key == KeyCode.Space)
        {
            GameObject galton = GameObject.FindGameObjectWithTag("Galton");
            galton.GetComponent<Galton>().end();
            galton.GetComponent<Galton>().start();
        }
    }
}
