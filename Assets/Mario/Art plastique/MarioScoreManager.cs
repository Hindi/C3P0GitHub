using UnityEngine;
using System.Collections;

public class MarioScoreManager : MonoBehaviour {

    private int score;
    public int Score
    {
        get { return score; }
    }

    [SerializeField]
    private PipesManager pipeManager;

    [SerializeField]
    private GameObject invisBorder;

    bool inPlace;

	// Use this for initialization
	void Start () {
        inPlace = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void restart()
    {
        inPlace = false;
        score = 0;
    }

    public void addScore(int s)
    {
        score += s;
        if (score > 40 && !inPlace)
        {
            inPlace = true;
            pipeManager.moveBigPipe(1);
            invisBorder.SetActive(false);
        }
        else if (score > 32)
            pipeManager.moveBigPipe(1);
        else if (score > 25)
            pipeManager.moveBigPipe(1);
        else if (score > 16)
            pipeManager.moveSmallPipe(1);
        else if (score > 8)
            pipeManager.moveBigPipe(1);
    }
}
