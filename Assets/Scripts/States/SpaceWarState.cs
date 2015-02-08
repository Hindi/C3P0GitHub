using UnityEngine;
using System.Collections;

/// <summary>
/// AI script for the enemy in SpaceWar
/// </summary>
public class SpaceWarState : GameState {

    /// <summary>
    /// The script managing most of the game's mechanics
    /// </summary>
    private SpaceWarScript gameScript;
    /// <summary>
    /// The player script
    /// </summary>
    private PlayerSpaceWar player;

    /// <summary>
    /// Called when the lobby scene from Unity is loaded, sets the Game ID
    /// </summary>
    /// <param name="stateManager">The instance managing all the states</param>
	public SpaceWarState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.SPACEWAR;
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
        gameScript = GameObject.FindGameObjectWithTag("SpaceWarScript").GetComponent<SpaceWarScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpaceWar>();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
        gameScript.onRestart();
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
    }

    /// <summary>
    /// Similar to Unity's OnDestroy delegate, called when the state stops
    /// </summary>
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

    /// <summary>
    /// Similar to Unity's Update delegate, called each frame to advance in game
    /// </summary>
    public override void update()
    {
        base.update();
        if (gameScript != null && score != gameScript.Score)
        {
            score = gameScript.Score;
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
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
                if (t.position.y < Screen.height / 2) // partie inférience de l'écran
                {
                    if (t.position.x < Screen.width / 2) // partie gauche de l'écran
                    {
                        player.rotate(1);
                    }
                    else // partie droite
                    {
                        player.goForward();
                    }
                }
                else // partie supérieure de l'écran
                {
                    if (t.position.x < Screen.width / 2) // partie gauche de l'écran
                    {
                        player.rotate(-1);
                    }
                    else // partie droite
                    {
                        player.fire();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called by the Input Manager to notify a key/touch screen action going on
    /// </summary>
    /// <param name="key">The key being pressed</param>
    /// <param name="inputs">A touch screen action</param>
    public override void noticeInput(EnumInput key)
    {
        if (loaded)
        {
            base.noticeInput(key);
            if (key == EnumInput.SPACE)
                player.fire();
            if (key == EnumInput.LEFT)
                player.rotate(1);
            else if (key == EnumInput.RIGHT)
                player.rotate(-1);
            if (key == EnumInput.UP)
                player.goForward();
        }
    }
}
