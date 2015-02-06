using UnityEngine;
using System.Collections;

/// <summary>This state is active when the player plays lunar lander.</summary>
class LunarLanderState : GameState
{
    /// <summary>The gameObject of the player.</summary>
    [SerializeField]
    private GameObject player_;

    /// <summary>The script of the player.</summary>
    private PlayerLunarLander playerScript_;

    /// <summary>The current terrain.</summary>
    private TerrainLunarLander terrain;

    /// <summary>Constructor.</summary>
    public LunarLanderState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.LUNARLANDER;
    }

    /// <summary>Called when the player restart the game.</summary>
    /// <returns>void</returns>
    public override void onGameRestart()
    {
        if (loaded)
            playerScript_.onGameRestart();
    }

    /// <summary>Define the active terrain depending on the id given in parameter.</summary>
    /// <param name="param">The parameter the player chose.</param>
    /// <returns>void</returns>
    public override void setParameter(Parameter param)
    {
        terrain.setTerrain(param.id);
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        player_ = GameObject.FindGameObjectWithTag("Player");
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainLunarLander>();
        playerScript_ = player_.GetComponent<PlayerLunarLander>();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
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
                if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if(t.position.x < Screen.width / 2)
                    {
                        if (t.position.y > Screen.height / 2)
                            playerScript_.rotate(-1);
                        else if (t.position.y < Screen.height / 2)
                            playerScript_.rotate(1);
                    }
                }
                if (t.phase == TouchPhase.Began)
                {
                    if (t.position.x > Screen.width / 2)
                    {
                        if (t.position.y > Screen.height / 2)
                            playerScript_.increaseReactorState();
                        else if (t.position.y < Screen.height / 2)
                            playerScript_.decreaseReactorState();
                    }
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
            if (!Application.isMobilePlatform)
            {
                base.noticeInput(key);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    playerScript_.increaseReactorState();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    playerScript_.decreaseReactorState();
                if (Input.GetKey(KeyCode.LeftArrow))
                    playerScript_.rotate(1);
                if (Input.GetKey(KeyCode.RightArrow))
                    playerScript_.rotate(-1);
            }
        }
    }
}
