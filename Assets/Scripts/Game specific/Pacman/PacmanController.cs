using UnityEngine;
using System.Collections;

/// <summary>
/// Pacman controller is the class that controls the behavour of each enemy, the score and the collectible elements.
/// </summary>
public class PacmanController : MonoBehaviour {

	/// <summary>
	/// The tile representation of the game board.
	/// </summary>
	int[,] pacGrid;

	/// <summary>
	/// The current number of dots on the screen
	/// </summary>
	int remainingDots = 0;

	/// <summary>
	/// The timer for the scatter cycle.
	/// </summary>
	float scatterDuration = 10f;
	float scatterDurationTimer = 0f;

	float scatterDelay = 40f;
	float scatterDelayTimer = 0f;

	float frightenTimer = 0f;
	float frightenDuration = 5f;


	/// <summary>
	/// The player's score.
	/// </summary>
	int score;

	/// <summary>
	/// The dot prefab.
	/// </summary>
	[SerializeField]
	private GameObject pacDot;

	/// <summary>
	/// The energizer prefab.
	/// </summary>
	[SerializeField]
	private GameObject Energizer;


	/// <summary>
	/// Deletes the dots present in the scene.
	/// </summary>
	void deleteDots(){
		GameObject[] dots = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject dot in dots){
			Destroy(dot);
			--remainingDots;
		}
		GameObject[] eners = GameObject.FindGameObjectsWithTag("Energizer");
		foreach(GameObject ener in eners){
			Destroy(ener);
			--remainingDots;
		}
	}
	/// <summary>
	/// Creates the dots.
	/// </summary>
	void createDots(){
		for (int i = 0; i<28; ++i){
			for(int j = 0; j<31; j++){
				if(pacGrid[j,i] == 1){
					GameObject dot = Instantiate(pacDot, new Vector3(i, 0.3f, -j), Quaternion.identity) as GameObject;
					dot.transform.parent = transform;
					++remainingDots;
				}
				if(pacGrid[j,i] == 3){
					GameObject ene = Instantiate(Energizer, new Vector3(i, .3f, -j), Quaternion.identity) as GameObject;
					ene.transform.parent = transform;
					++remainingDots;
				}
			}
		}
	}

	/// <summary>
	/// This function is called each time the player restarts the game.
	/// </summary>
	public void onRestart(){
		deleteDots();
		createDots();
		score = 0;
		EventManager.Raise(EnumEvent.RESTARTSTATE);
		EventManager.Raise(EnumEvent.GAMEOVER);
	}

	/// <summary>
	/// Called when the player has won or lost the game
	/// </summary>
	public void onGameOver(){
		EventManager.Raise(EnumEvent.GAMEOVER);
	}

	/// <summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		EventManager<bool>.AddListener(EnumEvent.FRIGHTENED, enerEaten);
		EventManager.RemoveListener(EnumEvent.DOT_EATEN, dotEaten);
		EventManager.RemoveListener(EnumEvent.GHOST_EATEN, ghostEaten);
	}
	/* Generating all the 244 pacDots on the field*/
	void Start () {
		pacGrid =new int[31,28]{
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 3, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 3, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 3, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		createDots();
		EventManager<bool>.AddListener(EnumEvent.FRIGHTENED, enerEaten);
		EventManager.AddListener(EnumEvent.DOT_EATEN, dotEaten);
		EventManager.AddListener(EnumEvent.GHOST_EATEN, ghostEaten);
	}
	
	/// <summary>
	/// Called when the player gets a dot.
	/// </summary>
	void dotEaten(){
		--remainingDots;
		score += 10;
		EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
		if (remainingDots == 0){
			EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
		}
	}

	/// <summary>
	/// Called when a player gets an energizer.
	/// </summary>
	void enerEaten(bool res){
		if (res){
		--remainingDots;
		score += 50;
			frightenTimer = 0f;
		EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
		if (remainingDots == 0){
			EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
		}
		}
	}

	/// <summary>
	/// Called when a player gets a froghtened ghost
	/// </summary>
	void ghostEaten(){
		score += 200;
		EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
	}

	/// <summary>
	/// Called at each frame.
	/// </summary>
	void Update () {
			scatterDelayTimer += Time.deltaTime;
			frightenTimer += Time.deltaTime;
			scatterDurationTimer += Time.deltaTime;
		if (scatterDelayTimer > scatterDelay)
			{
			scatterDurationTimer = 0f;
			EventManager<bool>.Raise(EnumEvent.SCATTERMODE, true);
			if (scatterDurationTimer > scatterDuration){
				EventManager<bool>.Raise(EnumEvent.SCATTERMODE, false);
				scatterDurationTimer = 0f;
				scatterDelayTimer = 0f;
			}
		}
		if(frightenTimer > frightenDuration)
		{
			EventManager<bool>.Raise(EnumEvent.FRIGHTENED, false);
		}
	}
}