using UnityEngine;
using System.Collections;

/// <summary>Abstract class inherited once for each game.</summary>
public abstract class GameState : State
{
    /// <summary>The time scale is saved here before a pause.</summary>
    private float timeScale = Time.timeScale;

    /// <summary>True if the game is paused.</summary>
    protected bool paused;

    /// <summary>The UI class that contains all the ui.</summary>
    protected UI ui;

    /// <summary>The current game id.</summary>
    protected EnumGame gameId;

    protected int score = 0;
    protected bool scoreChanged;

    /// <summary>The current parameter id.</summary>
    protected int paramId = 0;

    private const int sendScoreCD = 3;
    private float lastSendScoreTime;

    float answerRatio;
    

    /// <summary>Constructor.</summary>
    public GameState(StateManager stateManager) : base(stateManager)
    {
		paused = false;
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
    }

    /// <summary>Called when the player restart the game.</summary>
    /// <returns>void</returns>
    public virtual void onGameRestart()
    {

    }

    /// <summary>Called when the player lose or win.</summary>
    /// <param name="b">True if the player won.</param>
    /// <returns>void</returns>
    public virtual void onGameOver(bool b)
    {
        if (Network.isClient)
            C3PONetworkManager.Instance.sendNotifyGameOver(b);
        onGamePaused(true);
    }

    /// <summary>Called when the game is paused or unpaused.</summary>
    /// <param name="b">True if paused</param>
    /// <returns>void</returns>
	public void onGamePaused(bool b)
    {
        applyPause(b);
	}

    /// <summary>Abstract function specific for each game. It is called when the player chose a parameter</summary>
    /// <param name="param">The parameter the player chose.</param>
    /// <returns>void</returns>
    public abstract void setParameter(Parameter param);

    /// <summary>Pause or unpause the game.</summary>
    /// <param name="b">True if paused.</param>
    /// <returns>void</returns>
	public virtual void pauseGame(bool b)
    {
		EventManager<bool>.Raise (EnumEvent.PAUSEGAME, paused);
	}

    /// <summary>Toggle between pause and unpause.</summary>
    /// <returns>void</returns>
	public virtual void togglePauseGame()
	{
		EventManager<bool>.Raise (EnumEvent.PAUSEGAME, !paused);
	}

    /// <summary>Called when the player wants to change the parameter.</summary>
    /// <returns>void</returns>
    public virtual void changeParam()
    {
        applyPause(true);
        EventManager <float>.Raise(EnumEvent.CHANGEPARAM, answerRatio);
    }

    /// <summary>Set paused to the value of the given parameter and apply the pause or unpause.</summary>
    /// <param name="b">True if the game needs to be paused.</param>
    /// <returns>void</returns>
    protected void applyPause(bool b)
    {
        paused = b;
        applyPause();
    }

    /// <summary>Apply the pause effect by modifying the timescale.</summary>
    /// <returns>void</returns>
	private void applyPause()
	{
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = timeScale;
	}

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
    {
        lastSendScoreTime = Time.time;
        loaded = true;
        changeParam();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        EventManager<bool>.AddListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager<bool>.AddListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.AddListener(EnumEvent.RESTARTGAME, onGameRestart);
        EventManager<int>.AddListener(EnumEvent.UPDATEGAMESCORE, onScoreUpdate);
        EventManager<float>.AddListener(EnumEvent.GOODANSWERRATIO, goodAnswerRatio);
    }

    public void onScoreUpdate(int s)
    {
        scoreChanged = true;
        score = s;
    }

    /// <summary>Called when leaving this state.</summary>
    /// <returns>void</returns>
    public override void end()
    {
        loaded = false;
        EventManager<bool>.RemoveListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager<bool>.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.RemoveListener(EnumEvent.RESTARTGAME, onGameRestart);
        EventManager<int>.RemoveListener(EnumEvent.UPDATEGAMESCORE, onScoreUpdate);
        EventManager<float>.RemoveListener(EnumEvent.GOODANSWERRATIO, goodAnswerRatio);
    }

    public void goodAnswerRatio(float r)
    {
        answerRatio = r;
    }


    /// <summary>Called each frame.</summary>
    /// <returns>void</returns>
    public override void update()
    {
        if (Time.time - lastSendScoreTime > sendScoreCD && Network.isClient && scoreChanged)
        {
            scoreChanged = true;
            lastSendScoreTime = Time.time;
            C3PONetworkManager.Instance.sendGameStats(paramId, score);
        }
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key)
    {
        if (key == EnumInput.ESCAPE || key == EnumInput.MENU)
            togglePauseGame();
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
