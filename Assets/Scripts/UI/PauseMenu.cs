using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onResumeClick()
    {
        EventManager<bool>.Raise(EnumEvent.PAUSEGAME, false);
    }

    public void onRestartClick()
    {
        EventManager.Raise(EnumEvent.RESTARTGAME);
        onResumeClick();
    }
}
