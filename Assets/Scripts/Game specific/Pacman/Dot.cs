using UnityEngine;
using System.Collections;

/// <summary>
/// Dot is the class thatcontrols the behaviour of each PacDot in the game.
/// </summary>
public class Dot : MonoBehaviour {

	/// <summary>
	/// True if the game is over
	/// </summary>
	bool gameOver = false;

	/// <summary>
	/// Called whent he game is won or lost.
	/// </summary>
	void onGameOver(){
		gameOver = true;
	}

	/// <summary>
	/// Called when the scene is loaded.
	/// </summary>
	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}

	/// <summary>
	/// Called when this script is destroyed.
	/// </summary>
	void OnDestroy(){
		if(!gameOver){
			EventManager.Raise(EnumEvent.DOT_EATEN);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
	}
}
