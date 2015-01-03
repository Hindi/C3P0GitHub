using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	[SerializeField]
    private Canvas pauseMenu;
    [SerializeField]
    private Canvas paramMenu;
    [SerializeField]
    private Canvas gameOverMenu;

    private Canvas currentCanvas;

	// Use this for initialization
	void Start () {
		EventManager<bool>.AddListener (EnumEvent.PAUSEGAME, onGamePaused);
        EventManager.AddListener(EnumEvent.CHANGEPARAM, onChangeParam);
        EventManager<bool>.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}


    public void onGameOver(bool b)
    {
        updateCurrentCanvas(gameOverMenu);
    }

	public void onGamePaused(bool b)
	{
        if (b)
            updateCurrentCanvas(pauseMenu);
        else
            closeMenus();
	}

    public void onChangeParam()
    {
        updateCurrentCanvas(paramMenu);
    }

    public void switchToParam()
    {
        updateCurrentCanvas(paramMenu);
    }

    private void updateCurrentCanvas(Canvas newCanvas)
    {
        if (currentCanvas)
            currentCanvas.gameObject.SetActive(false);
        currentCanvas = newCanvas;
        currentCanvas.gameObject.SetActive(true);
    }

    private void closeMenus()
    {
        gameOverMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        paramMenu.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
