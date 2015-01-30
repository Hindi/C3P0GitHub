using UnityEngine;
using System.Collections;


class SpaceInvaderState : GameState
{
    [SerializeField]
    private GameObject player_;
    private Player playerScript_;

    private int paramId;

    public SpaceInvaderState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.SPACEINVADER;
    }

    public override void onGameRestart()
    {
        if(loaded)
            playerScript_.onGameRestart();
    }

    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
            C3PONetworkManager.Instance.sendGameStats((int)gameId, paramId, playerScript_.Score);
        }
    }

    public override void setParameter(Parameter param)
    {
        paramId = param.id;
        player_.GetComponent<Player>().setParamId(param.id);
    }

	public override void onLevelWasLoaded(int lvl)
	{
        base.onLevelWasLoaded(lvl);
		loaded = true;
		player_ = GameObject.FindGameObjectWithTag("Player");
        playerScript_ = player_.GetComponent<Player>();
        ui.setParamCanvas(gameId);

        if(Application.isMobilePlatform)
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
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
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
