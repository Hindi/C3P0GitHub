﻿using UnityEngine;
using System.Collections;

/// <summary>This state is active when the player plays Space Invader.</summary>
class SpaceInvaderState : GameState
{
    /// <summary>The gameObject of the player.</summary>
    [SerializeField]
    private GameObject player_;

    /// <summary>The script of the player.</summary>
    private Player playerScript_;


    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public SpaceInvaderState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.SPACEINVADER;
    }

    /// <summary>Called when the player restart the game.</summary>
    /// <returns>void</returns>
    public override void onGameRestart()
    {
        if(loaded)
            playerScript_.onGameRestart();
    }

    /// <summary>Called when the player lose or win.</summary>
    /// <param name="b">True if the player won.</param>
    /// <returns>void</returns>
    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
        }
    }

    /// <summary>Defines which function will influence the size of the player.</summary>
    /// <param name="param">The parameter the player chose.</param>
    /// <returns>void</returns>
    public override void setParameter(Parameter param)
    {
        paramId = param.id;
        player_.GetComponent<Player>().setParamId(param.id);
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
	public override void onLevelWasLoaded(int lvl)
	{
        base.onLevelWasLoaded(lvl);
		loaded = true;
		player_ = GameObject.FindGameObjectWithTag("Player");
        playerScript_ = player_.GetComponent<Player>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        ui.setParamCanvas(gameId);
        setParameter(new Parameter());

        if(Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
	}

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
	{
        base.start();
    }

    /// <summary>Called when leaving this state.</summary>
    /// <returns>void</returns>
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

    /// <summary>Called each frame.</summary>
    /// <returns>void</returns>
    public override void update()
    {
        base.update();
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if (t.position.x > 3 * Screen.width / 4)
                        playerScript_.move(1);
                    else if (t.position.x < Screen.width / 4)
                        playerScript_.move(-1);
                    else if (!paused)
                        playerScript_.fire();
                }
            }
        }
    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key)
    {
       	if (loaded)
        {
            base.noticeInput(key);
            if (key == EnumInput.SPACE)
                playerScript_.fire();
            if (key == EnumInput.LEFT)
                playerScript_.move(-1);
            else if (key == EnumInput.RIGHT)
                playerScript_.move(1);
		}
    }
}
