using UnityEngine;
using System.Collections;


class SpaceInvaderState : GameState
{
    [SerializeField]
    private GameObject player_;
    private Player playerScript_;

    public SpaceInvaderState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void onGameRestart()
    {
        if(loaded)
            playerScript_.onGameRestart();
    }

    public override void setParameter(Parameter param)
        {
            player_.GetComponent<Player>().setParamId(param.id);
    }

	public override void onLevelWasLoaded(int lvl)
	{
        base.onLevelWasLoaded(lvl);
		loaded = true;
		player_ = GameObject.FindGameObjectWithTag("Player");
		playerScript_ = player_.GetComponent<Player>();
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
			if (key == KeyCode.Space)
				playerScript_.fire ();
			if (key == KeyCode.Return)
			{

			}
		}
    }
}
