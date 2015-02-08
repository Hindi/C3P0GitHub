using UnityEngine;
using System.Collections;

public class PongState : GameState {

    /// <summary>
    /// The script managing most of the game's mechanics
    /// </summary>
    private PongManagerScript gameScript;
    private Parameter p;
    /// <summary>
    /// The player script
    /// </summary>
    private PlayerControl player;

    /// <summary>
    /// Called when the lobby scene from Unity is loaded, sets the Game ID
    /// </summary>
    /// <param name="stateManager">The instance managing all the states</param>
    public PongState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.PONG;
    }

    /// <summary>
    /// Delegate called when the game needs to (re)start
    /// </summary>
    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
            gameScript.onRestart();
        }
    }

    /// <summary>
    /// Sets the parameter used in game when selected from the menu
    /// </summary>
    /// <param name="param">The parameter that should be used in game forward</param>
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

    /// <summary>
    /// Delegate called when the game scene is done loading.
    /// </summary>
    /// <param name="lvl">The game's level done loading</param>
    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        gameScript = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
        gameScript.updateElementsResolution();
        setParameter(new Parameter());
    }

    /// <summary>
    /// Similar to Unity's Start delegate, called when the state starts
    /// </summary>
    public override void start()
    {
        base.start();
        ui.setParamCanvas(gameId);
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// Similar to Unity's OnDestroy delegate, called when the state stops
    /// </summary>
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
        Application.targetFrameRate = -1;
    }

    /// <summary>
    /// Similar to Unity's Update delegate, called each frame to advance in game
    /// </summary>
    public override void update()
    {
        base.update();
        if (gameScript != null && score != gameScript.playerScore - gameScript.enemyScore)
        {
            score = gameScript.playerScore - gameScript.enemyScore;
            EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
        }
    }

    /// <summary>
    /// Delegate called when the game is over
    /// </summary>
    /// <param name="b">True if player won</param>
    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
        }
    }

    /// <summary>
    /// Called by the Input Manager to notify a key is being pressed
    /// </summary>
    /// <param name="key">The key being pressed</param>
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

    /// <summary>
    /// Called by the Input Manager to notify a key/touch screen action going on
    /// </summary>
    /// <param name="key">The key being pressed</param>
    /// <param name="inputs">A touch screen action</param>
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
