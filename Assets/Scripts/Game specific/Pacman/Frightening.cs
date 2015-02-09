using UnityEngine;
using System.Collections;

public class Frightening : MonoBehaviour {

	/// <summary>
	/// True if the game is over
	/// </summary>
	bool gameOver = false;
	Behaviour halo;


	/// <summary>
	/// Called when the game is over
	/// </summary>
	void onGameOver(){
		gameOver = true;
	}

	/// <summary>
	/// Called when the player wins the mini-game and this ghost is his encounter
	/// </summary>
	/// <param name="tag">The tage of the encounter.</param>
	void sentenceWin(GameObject obj){
		halo.enabled = true;
		if (obj == gameObject){
			Destroy(gameObject);
		}
	}
	
	/// <summary>
	/// Called when the time is over and this ghost is his encounter
	/// </summary>
	/// <param name="tag">The tag of the encounter.</param>
	void sentenceTO(GameObject obj){
		halo.enabled = true;
	}
	
	void sentenceLost(GameObject obj){
		halo.enabled = true;	
	}

	void onStartMiniGame(){
		halo.enabled = false;
	}

	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_LOST, sentenceLost);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.AddListener(EnumEvent.MINIGAME_START, onStartMiniGame);
		halo = GetComponent("Halo") as Behaviour;
	}

	///<summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		if(!gameOver){
			EventManager<bool>.Raise(EnumEvent.FRIGHTENED, true);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
		EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_LOST, sentenceLost);
		EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.RemoveListener(EnumEvent.MINIGAME_START, onStartMiniGame);

	}
}
