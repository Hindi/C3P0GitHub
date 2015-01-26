using UnityEngine;
using System.Collections;

public class GameChoiceMenu : MonoBehaviour {

    [SerializeField]
    private LevelLoader levelLoader;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadLevel(string level)
    {
        levelLoader.loadLevel(level);
    }
}
