using UnityEngine;
using System.Collections;

/**
 * One of the game states, will probably require another level of inheritence.
 * 
 */

public class GameState : State
{

	private bool paused;
	private float timeScale = Time.timeScale;

    public GameState(StateManager stateManager) : base(stateManager)
    {
		paused = false;
		EventManager<bool>.AddListener (EnumEvent.PAUSEGAME, onGamePaused);
    }

	public void onGamePaused(bool b)
	{
		paused = b;
		applyPause ();
	}

    // Use this for initialization
    public override void start()
    {

    }

    public virtual void setParameter(Parameter param)
    {
        Debug.Log(param.id);
    }

	public virtual void pauseGame(bool b)
	{
		EventManager<bool>.Raise (EnumEvent.PAUSEGAME, paused);
	}

	public virtual void togglePauseGame()
	{
		EventManager<bool>.Raise (EnumEvent.PAUSEGAME, !paused);
	}

	private void applyPause()
	{
		if (paused)
			Time.timeScale = 0;
		else
			Time.timeScale = timeScale;
	}

    // Use this for state transition
    public override void end()
    {

    }

    // Update is called once per frame
    public override void update()
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }
}
