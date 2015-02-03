using UnityEngine;
using System.Collections;

public class MarioScoreManager : MonoBehaviour {

    private int score;

    [SerializeField]
    private PipesManager pipeManager;

    [SerializeField]
    private GameObject invisBorder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addScore(int s)
    {
        score += s;
        if (score > 40)
        {
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
