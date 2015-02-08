using UnityEngine;
using System.Collections;

public class Frightening : MonoBehaviour {

	/// <summary>
	/// True if the game is over
	/// </summary>
	bool gameOver = false;

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
	void sentenceWin(string tag){
		renderer.enabled = true;
		if (tag == gameObject.tag){
			Destroy(gameObject);
		}
	}
	
	/// <summary>
	/// Called when the time is over and this ghost is his encounter
	/// </summary>
	/// <param name="tag">The tag of the encounter.</param>
	void sentenceTO(string tag){
		renderer.enabled = true;
	}
	
	void sentenceLost(string tag){
		renderer.enabled = true;	
	}

	void onStartMiniGame(){
		renderer.enabled = false;
	}

	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_LOST, sentenceLost);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.AddListener(EnumEvent.MINIGAME_START, onStartMiniGame);
	}

	///<summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		if(!gameOver){
			EventManager.Raise(EnumEvent.FRIGHTENED);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_LOST, sentenceLost);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.RemoveListener(EnumEvent.MINIGAME_START, onStartMiniGame);

	}
}
