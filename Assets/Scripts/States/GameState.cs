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
    protected bool loaded;
    protected UI ui;
    protected EnumGame gameId;

    public GameState(StateManager stateManager) : base(stateManager)
    {
		paused = false;
        EventManager<bool>.AddListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager<bool>.AddListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.AddListener(EnumEvent.RESTARTGAME, onGameRestart);
    }

    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    public virtual void onGameRestart()
    {

    }

    public virtual void onGameOver(bool b)
    {
        onGamePaused(true);
    }

	public void onGamePaused(bool b)
	{
		paused = b;
		applyPause ();
	}

    // Use this for initialization
    public override void start()
    {
        changeParam();
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

    public virtual void changeParam()
    {
        applyPause(true);
        EventManager.Raise(EnumEvent.CHANGEPARAM);
    }

    private void applyPause(bool b)
    {
        paused = b;
        applyPause();
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
        EventManager<bool>.RemoveListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager<bool>.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.RemoveListener(EnumEvent.RESTARTGAME, onGameRestart);

        loaded = false;
    }

    // Update is called once per frame
    public override void update()
    {

    }

    public override void noticeInput(EnumInput key)
    {
        if (key == EnumInput.ESCAPE || key == EnumInput.MENU)
            togglePauseGame();
    }

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
