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

    public override void noticeInput(KeyCode key)
    {
        if (loaded)
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
