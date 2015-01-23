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
    }

    // Update is called once per frame
    public override void update()
    {
        base.update();
        if (Application.isMobilePlatform)
        {
            foreach (var t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    if (t.position.x > 2 * Screen.width / 3)
                        playerScript_.setDirection(true);
                    else if (t.position.x < Screen.width / 3)
                        playerScript_.setDirection(false);
                    else
                        playerScript_.fire();
                }
                if (t.phase == TouchPhase.Ended)
                {
                    if (t.position.x > 2 * Screen.width / 3 || t.position.x < Screen.width / 3)
                        playerScript_.stop();
                }
            }
        }
    }

    public override void noticeInput(KeyCode key)
    {
       	if (loaded)
        {
            base.noticeInput(key);
            if (!Application.isMobilePlatform)
            {
                if (key == KeyCode.Space)
                    playerScript_.fire();
                if (Input.GetKey(KeyCode.LeftArrow))
                    playerScript_.setDirection(false);
                else if (Input.GetKey(KeyCode.RightArrow))
                    playerScript_.setDirection(true);
                else
                    playerScript_.stop();
            }
		}
    }
}
