using UnityEngine;
using System.Collections;

/// <summary>This state is active when the player plays Space Invader.</summary>
class PacmanState : GameState
{
	/// <summary>The gameObject of the player.</summary>
	[SerializeField]
	private GameObject player_;

	[SerializeField]
	private GameObject gameController_;

	[SerializeField]
	private GameObject circleCamera;

	/// <summary>The script of the player.</summary>
	private PacMove playerScript_;

	private PacmanController gameControllerScript_;

	private Randomizer rand;
	
	
	/// <summary>Called when the lobby scene from Unity is loaded.</summary>
	/// <param name="lvl">Id of the level loaded.</param>
	/// <returns>void</returns>
	public PacmanState(StateManager stateManager)
		: base(stateManager)
	{
		gameId = EnumGame.PACMAN;
	}
	
	/// <summary>Called when the player restart the game.</summary>
	/// <returns>void</returns>
	public override void onGameRestart()
	{
		if(loaded){
			gameControllerScript_.onRestart();
		}
	}
	
	/// <summary>Called when the player lose or win.</summary>
	/// <param name="b">True if the player won.</param>
	/// <returns>void</returns>
	public override void onGameOver(bool b)
	{
		if (loaded)
		{
			base.onGameOver(b);
			gameControllerScript_.onRestart();
		}
	}
	
	/// <summary>Defines which function will influence the size of the player.</summary>
	/// <param name="param">The parameter the player chose.</param>
	/// <returns>void</returns>
	public override void setParameter(Parameter param)
	{
		paramId = param.id;
		rand.setParamId(param.id);
	}
	
	/// <summary>Called when the lobby scene from Unity is loaded.</summary>
	/// <param name="lvl">Id of the level loaded.</param>
	/// <returns>void</returns>
	public override void onLevelWasLoaded(int lvl)
	{
		base.onLevelWasLoaded(lvl);
		loaded = true;
		player_ = GameObject.FindGameObjectWithTag("Player");
		playerScript_ = player_.GetComponent<PacMove>();
		gameController_ = GameObject.FindGameObjectWithTag("Pacman");
		gameControllerScript_ = gameController_.GetComponent<PacmanController>();
		circleCamera = GameObject.FindGameObjectWithTag("CircleCamera");
		rand = circleCamera.GetComponent<Randomizer>();

		if(Application.isMobilePlatform)
			Screen.orientation = ScreenOrientation.Portrait;
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
		if (Application.isMobilePlatform)
			Screen.orientation = ScreenOrientation.AutoRotation;
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
				if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
				{
					if (t.position.y > 2 * Screen.height /3)
						playerScript_.goUp();
					else if (t.position.y < Screen.height / 4)
						playerScript_.goDown();
					else if (t.position.x < Screen.width / 2)
						playerScript_.goLeft();
					else if (t.position.x < Screen.width / 2)
						playerScript_.goRight();
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
			base.noticeInput(key);
			if (key == EnumInput.UP)
				playerScript_.goUp();
			if (key == EnumInput.LEFT)
				playerScript_.goLeft();
			if (key == EnumInput.RIGHT)
				playerScript_.goRight();
			if (key == EnumInput.DOWN)
				playerScript_.goDown();
		}
	}
}

