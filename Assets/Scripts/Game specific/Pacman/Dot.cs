using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour {

	bool gameOver = false;
	
	void onGameOver(){
		gameOver = true;
	}
	
	void Start(){
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}
	void OnDestroy(){
		if(!gameOver){
			EventManager.Raise(EnumEvent.DOT_EATEN);
		}
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
	}
}
