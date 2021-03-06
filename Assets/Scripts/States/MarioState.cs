﻿using UnityEngine;
using System.Collections;

public class MarioState : GameState 
{
    /// <summary>The gameObject of the player.</summary>
    private GameObject player_;

    /// <summary>The script of the player.</summary>
    private FirstPersonController playerScript_;
    private MarioScoreManager scoreManager;
    private MarioSpawner spawner;

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public MarioState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.MARIO;
    }

    /// <summary>Called when the player restart the game.</summary>
    /// <returns>void</returns>
    public override void onGameRestart()
    {
        if (loaded)
        {
            playerScript_.restart();
            scoreManager.restart();
        }
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
        spawner.ParamId = paramId;
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        player_ = GameObject.FindGameObjectWithTag("Player");
        playerScript_ = player_.GetComponent<FirstPersonController>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MarioScoreManager>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MarioSpawner>();

        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
        setParameter(new Parameter());
    }

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
    {
        base.start();
        ui.setParamCanvas(gameId);
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
            base.noticeInput(key, inputs);
            foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if (t.position.y < Screen.height / 2)
                    {
                        if (t.position.x > Screen.width / 2)
                            playerScript_.moveRight();
                        else if (t.position.x < Screen.width / 2)
                            playerScript_.moveLeft();
                    }
                    else
                        playerScript_.jump();
                }
                else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                    if (t.position.y < Screen.height / 2)
                        playerScript_.stop();
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
                playerScript_.jump();
            if (key == EnumInput.LEFT)
                playerScript_.moveLeft();
            else if (key == EnumInput.RIGHT)
                playerScript_.moveRight();
            else
                playerScript_.stop();
        }
    }
}
