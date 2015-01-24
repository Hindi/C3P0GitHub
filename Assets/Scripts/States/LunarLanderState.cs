using UnityEngine;
using System.Collections;


class LunarLanderState : GameState
{
    [SerializeField]
    private GameObject player_;
    private PlayerLunarLander playerScript_;
    private TerrainLunarLander terrain;
    private bool loaded;

    public LunarLanderState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.LUNARLANDER;
    }

    public override void onGameRestart()
    {
        if (loaded)
            playerScript_.onGameRestart();
    }

    public override void setParameter(Parameter param)
    {
        terrain.setTerrain(param.id);
    }

    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        loaded = true;
        player_ = GameObject.FindGameObjectWithTag("Player");
        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainLunarLander>();
        playerScript_ = player_.GetComponent<PlayerLunarLander>();
        ui.setParamCanvas(gameId);
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.Landscape;
    }

    // Use this for initialization
    public override void start()
    {
        base.start();
    }


    // Use this for state transition
    public override void end()
    {
        base.end();
    }

    // Update is called once per frame
    public override void update()
    {
        base.update();

    }

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if (t.position.x > 2 * Screen.width / 3)
                        playerScript_.rotate(-1);
                    else if (t.position.x < Screen.width / 3)
                        playerScript_.rotate(1);
                }
                else if (t.phase == TouchPhase.Began)
                {
                    if (t.position.y > 2 * Screen.height / 3)
                        playerScript_.increaseReactorState();
                    else if (t.position.y < Screen.height / 3)
                        playerScript_.decreaseReactorState();
                }
            }
        }
    }

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
