using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	[SerializeField]
	private Canvas pauseMenu;

	// Use this for initialization
	void Start () {
		EventManager<bool>.AddListener (EnumEvent.PAUSEGAME, onGamePaused);
	}

	public void onGamePaused(bool b)
	{
		if(b)
			pauseMenu.gameObject.SetActive(true);
		else
			pauseMenu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
