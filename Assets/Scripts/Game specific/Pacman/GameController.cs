using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	int[,] pacGrid;
	int remainingDots = 0;
	float timer;
	int score;
	int scoreAux;
	[SerializeField]
	private GameObject pacDot;

	[SerializeField]
	private GameObject Energizer;

	void deleteDots(){
		scoreAux = score;
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
		score = scoreAux;
	}

	void createDots(){
			deleteDots();
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

	void onGameOver(){
		score = 0;
		scoreAux = 0;
	}

	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.FRIGHTENED, enerEaten);
		EventManager.RemoveListener(EnumEvent.DOT_EATEN, dotEaten);
		EventManager.RemoveListener(EnumEvent.GHOST_EATEN, ghostEaten);
		EventManager.RemoveListener(EnumEvent.RESTARTGAME, createDots);
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
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
		timer = Time.time;
		createDots();
		EventManager.AddListener(EnumEvent.FRIGHTENED, enerEaten);
		EventManager.AddListener(EnumEvent.DOT_EATEN, dotEaten);
		EventManager.AddListener(EnumEvent.GHOST_EATEN, ghostEaten);
		EventManager.AddListener(EnumEvent.RESTARTGAME, createDots);
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}

	void dotEaten(){
		--remainingDots;
		Debug.Log(remainingDots);
		score += 10;
		if (remainingDots == 0){
			EventManager.Raise(EnumEvent.RESTARTGAME);
			EventManager.Raise(EnumEvent.RESTARTSTATE);
		}
	}

	void enerEaten(){
		--remainingDots;
		Debug.Log(remainingDots);
		score += 50;
		if (remainingDots == 0){
			EventManager.Raise(EnumEvent.RESTARTGAME);
			EventManager.Raise(EnumEvent.RESTARTSTATE);
		}
	}

	void ghostEaten(){
		score += 200;
	}

	
	// Update is called once per frame
	void Update () {
		if (Time.time - timer > 40f)
		{
			timer = Time.time;
			EventManager.Raise(EnumEvent.SCATTERMODE);
		}

	}

}
