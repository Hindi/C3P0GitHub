﻿using UnityEngine;
using System.Collections;

public class PongState : GameState {

    private PongManagerScript gameScript;
    private Parameter p;
    private PlayerControl player;

    public PongState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.PONG;
    }

    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
            gameScript.onRestart();
        }
    }

    public override void setParameter(Parameter param)
    {
        if (gameScript != null)
        {
            gameScript.setParameter(param);
            p = param;
        }
        else
        {
            Debug.Log("gameScript wasn't yet set when trying to set parameter");
        }
    }

    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        gameScript = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
        gameScript.updateElementsResolution();
    }

    public override void start()
    {
        base.start();
        ui.setParamCanvas(gameId);
    }

    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public override void update()
    {
        base.update();
        if (gameScript != null && score != gameScript.playerScore - gameScript.enemyScore)
        {
            score = gameScript.playerScore - gameScript.enemyScore;
            EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
        }
    }

    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
        }
    }

    public override void noticeInput(EnumInput key)
    {
        if (loaded)
        {
            base.noticeInput(key);
            if (key == EnumInput.SPACE)
                gameScript.launchCoupSpecial(-1);
            if (key == EnumInput.UP)
                player.goUp();
            if (key == EnumInput.DOWN)
                player.goDown();
        }
    }

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if (t.position.x < Screen.width / 2)
                    {
                        if (t.position.y > Screen.height / 2)
                            player.goUp();
                        else
                            player.goDown();
                    }
                    else
                        gameScript.launchCoupSpecial(-1);
                }
            }
        }
    }

}
