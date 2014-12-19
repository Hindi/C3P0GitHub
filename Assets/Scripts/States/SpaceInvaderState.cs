using UnityEngine;
using System.Collections;


class SpaceInvaderState : GameState
{
    [SerializeField]
    private GameObject player_;
    private Player playerScript_;
	private bool loaded;

    public SpaceInvaderState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void setParameter(Parameter param)
    {
        Debug.Log(param.id);
    }

	public override void onLevelWasLoaded(int lvl)
	{
		loaded = true;
		player_ = GameObject.FindGameObjectWithTag("Player");
		playerScript_ = player_.GetComponent<Player>();
	}

    // Use this for initialization
    public override void start()
	{

    }


    // Use this for state transition
    public override void end()
    {

    }

    // Update is called once per frame
    public override void update()
    {

    }

    public override void noticeInput(KeyCode key)
    {
       	if (loaded) 
		{
			if(key == KeyCode.Escape)
				togglePauseGame ();
			if (key == KeyCode.Space)
				playerScript_.fire ();
			if (key == KeyCode.Return)
			{

			}
		}
    }
}
