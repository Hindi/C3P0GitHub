using UnityEngine;
using System.Collections;

public class Frightening : MonoBehaviour {
	bool gameOver = false;

	void onGameOver(){
		gameOver = true;
	}

	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}

	void OnDestroy(){
		if(!gameOver){
			EventManager.Raise(EnumEvent.FRIGHTENED);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
	}
}
