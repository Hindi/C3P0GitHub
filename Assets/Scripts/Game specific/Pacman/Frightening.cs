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
	/// Called when the scene is loaded
	/// </summary>
	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}

	///<summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		if(!gameOver){
			EventManager.Raise(EnumEvent.FRIGHTENED);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
	}
}
